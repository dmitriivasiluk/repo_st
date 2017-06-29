using System.Diagnostics;

namespace SourceTree.AutomationTests.Utils.Helpers
{
    public class MercurialWrapper
    {
        private string _hgPath;
        public MercurialWrapper(string hgPath)
        {
            _hgPath = hgPath;
        }

        public bool Init(string pathToTestHgFolder)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = _hgPath;
            p.StartInfo.Arguments = $"init {pathToTestHgFolder}";
            p.Start();
            Error = p.StandardError.ReadToEnd();
            Output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return p.ExitCode == 0;
        }

        public string Error { get; private set; }
        public string Output { get; private set; }
    }
}
