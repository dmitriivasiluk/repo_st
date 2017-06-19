using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenObjectsHelpers.Helpers
{
    class AttemptsCounterLogger
    {
        public static void AttemptCounter(string nameOfMethod, int counter)
        {
            var output = nameOfMethod + counter;
            
            string path = Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\atteptsCounter.txt");

            File.AppendAllText(path, output);
        }
    }
}
