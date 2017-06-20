using System;
using System.IO;

namespace ScreenObjectsHelpers.Helpers
{
    public class AttemptsCounterLogger
    {
        public static void AttemptCounter(string nameOfMethod, string testContextFullName, int counter)
        {
            //var output = Environment.NewLine + testContextFullName + " -> " + nameOfMethod + " -> " + counter + " -> [" + DateTime.Now.ToString("h:mm:ss tt");

            string output = string.Format("[{0}] {1} -> {2} -> Attempts: {3}", DateTime.Now.ToString("h:mm:ss"), testContextFullName, nameOfMethod, counter) + Environment.NewLine;
            string path = Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\attemptsCounter.txt");

            File.AppendAllText(path, output);
        }
    }
}
