using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace AutomationTestsSolution.Helpers
{
    public class EmbeddedHgHelper
    {
        private string _downloadFolder;
        private string _userDataFolder;
        private string _toolsFolder;
        private Version _version;

        public EmbeddedHgHelper(string downloadFolder, string userDataFolder, string appPath, System.Version version)
        {
            _downloadFolder = downloadFolder;
            _userDataFolder = userDataFolder;
            _toolsFolder = Path.Combine(appPath, "tools");
            _version = version;
        }

        public bool Download(bool forceCleanRun)
        {
            InstallerPath = Path.Combine(_downloadFolder, "PortableHg.7z.exe");

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
                var url = $"https://downloads.atlassian.com/software/sourcetree/windows/PortableHg_{_version}.7z";
                var lineCount = 0;
                using (var stream = client.OpenRead(url))
                {
                    if (stream != null)
                    {
                        stream.ReadTimeout = Timeout.Infinite;

                        WebHeaderCollection whc = client.ResponseHeaders;
                        string contentLength = whc["Content-Length"];

                        int totalLength = (Int32.Parse(contentLength));
                        int fivePercent = ((totalLength) / 10) / 2;

                        //buffer of 5% of stream
                        byte[] fivePercentBuffer = new byte[fivePercent];

                        using (FileStream fs = new FileStream(InstallerPath, FileMode.Create, FileAccess.ReadWrite))
                        {
                            int count;
                            do
                            {
                                count = stream.Read(fivePercentBuffer, 0, fivePercent);
                                fs.Write(fivePercentBuffer, 0, count);
                            } while (count > 0);
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

        public bool InstallHg(bool forceCleanRun)
        {
            InstalledPath = Path.Combine(_userDataFolder, "hg_local");
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