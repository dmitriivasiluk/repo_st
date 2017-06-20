using System;
using System.IO;

namespace ScreenObjectsHelpers.Helpers
{
    public class AttemptsCounterLogger
    {
        public static void AttemptCounter(string nameOfMethod, string testContextFullName, int counter)
        {
            string output = string.Format("[{0}] {1} -> {2} -> Attempts: {3}", DateTime.Now.ToString("h:mm:ss dd.MM.yyyy"), testContextFullName.Replace("AutomationTestsSolution.Tests.", ""), nameOfMethod, counter) + Environment.NewLine;

            string path = Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\attemptsCounter.txt");

            File.AppendAllText(path, output);
        }
    }
}
