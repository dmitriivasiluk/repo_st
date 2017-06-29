using System.Diagnostics;
using System.Text;

namespace SourceTree.AutomationTests.Utils.Helpers
{
    public class ProcessHelper
    {
        private object _outputLock;
        private string _commandPath;
        private string _commandArgs;
        private StringBuilder _output = new StringBuilder();
        private StringBuilder _error = new StringBuilder();

        public string Output { get { return _output.ToString(); } }
        public string Error { get { return _error.ToString(); } }

        public ProcessHelper(string commandPath, string commandArgs)
        {
            _commandPath = commandPath;
            _commandArgs = commandArgs;
        }

        public bool Run()
        {
            _output = new StringBuilder();
            _error = new StringBuilder();

            var psi = new ProcessStartInfo(_commandPath, _commandArgs);
            psi.CreateNoWindow = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardInput = true;
            psi.UseShellExecute = false;
            var p = new Process();
            p.OutputDataReceived += OutputReceived;
            p.ErrorDataReceived += ErrorReceived;
            p.StartInfo = psi;
            p.Start();
            if (!p.WaitForExit(120000))
            {
                // did not finish
                p.Kill();
            }
            return p.ExitCode == 0;
        }

        private void OutputReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                var data = e.Data;

                lock (_outputLock)
                {
                    _output.AppendFormat("{0}\n", data);
                }
            }
        }

        private void ErrorReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                var data = e.Data;

                lock (_outputLock)
                {
                    _error.AppendFormat("{0}\n", data);
                }
            }
        }
    }
}