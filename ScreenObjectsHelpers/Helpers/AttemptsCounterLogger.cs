using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenObjectsHelpers.Helpers
{
    public class AttemptsCounterLogger
    {
        public static void AttemptCounter(string nameOfMethod, string testName, int counter)
        {
            var output = Environment.NewLine + testName + " -> " + nameOfMethod + " -> " + counter;
            
            string path = Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\attemptsCounter.txt");

            File.AppendAllText(path, output);
            
        }
    }
}
