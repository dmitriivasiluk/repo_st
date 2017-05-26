using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace AutomationTestsSolution.Helpers
{
    public class EmbeddedGitHelper
    {
        private string _downloadFolder;
        private string _userDataFolder;
        private string _toolsFolder;
        private Version _version;

        public EmbeddedGitHelper(string downloadFolder, string userDataFolder, string appPath, System.Version version)
        {
            _downloadFolder = downloadFolder;
            _userDataFolder = userDataFolder;
            _toolsFolder = Path.Combine(appPath, "tools");
            _version = version;
        }

        public bool Download(bool forceCleanRun)
        {
            InstallerPath = Path.Combine(_downloadFolder, "PortableGit.7z.exe");

            if (File.Exists(InstallerPath) && !forceCleanRun)
            {
                return true;
            }

            if (!Directory.Exists(_downloadFolder))
            {
                Directory.CreateDirectory(_downloadFolder);
            }

            using (var client = new System.Net.WebClient())
            {
                var url = $"https://downloads.atlassian.com/software/sourcetree/windows/PortableGit-{_version}-32-bit.7z.exe";
                var count = 0;
                using (var stream = client.OpenRead(url))
                {
                    if (stream != null)
                    {
                        stream.ReadTimeout = Timeout.Infinite;
                        using (var reader = new StreamReader(stream, Encoding.UTF8, false))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line != String.Empty)
                                {
                                    Console.WriteLine("Count {0}", count++);
                                }
                                Console.WriteLine(line);
                            }
                        }
                    }
                }
            }

            if (!File.Exists(InstallerPath))
            {
                return false;
            }

            return true;
        }

        public string InstallerPath { get; private set; }
        public string InstalledPath { get; private set; }

        public bool InstallGit(bool forceCleanRun)
        {
            InstalledPath = Path.Combine(_userDataFolder, "git_local");
            if (Directory.Exists(InstalledPath) && !forceCleanRun)
            {
                return true;
            }
            
            var commandParams = "x " + InstallerPath + " -y -o" + InstalledPath;
            var sevenZipExe = Path.Combine(_toolsFolder, @"7z.exe");
            return StartProcess(sevenZipExe, commandParams);
        }

        private bool StartProcess(string commandPath, string commandParams)
        {
            var psi = new ProcessStartInfo(commandPath, commandParams);
            var p  = new Process();
            p.StartInfo = psi;

            p.Start();
            p.WaitForExit(120000);
            return p.ExitCode == 0;
        }
    }
}