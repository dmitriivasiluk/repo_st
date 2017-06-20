using System.Diagnostics;
using System.IO;

namespace AutomationTestsSolution.Helpers
{
    public class SevenZipHelper
    {
        private string _toolsFolder;

        public SevenZipHelper(string appPath)
        {
            _toolsFolder = Path.Combine(appPath, "tools");
        }

        public bool Unzip(string zipFile, string targetPath, bool forceCleanRun = false)
        {
            if (Directory.Exists(targetPath) && !forceCleanRun)
            {
                return true;
            }

            var commandParams = "x " + zipFile + " -y -o" + targetPath;
            var sevenZipExe = Path.Combine(_toolsFolder, @"7z.exe");
            var ph = new ProcessHelper(sevenZipExe, commandParams);
            return ph.Run();
        }
    }
}