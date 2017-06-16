using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using System.Linq;

namespace ScreenObjectsHelpers.Helpers
{
    public class Utils
    {

        public static Window FindNewWindow(string nameOfWindow)
        {
            Window window = null;

            var attempt = 0;

            do
            {
                window = Desktop.Instance.Windows().FirstOrDefault(x => x.Name == nameOfWindow);

                Thread.Sleep(2000);
                attempt++;
            } while (window == null && attempt < 15);

            if (window == null)
            {
                Console.WriteLine("*** *** *** *** *** *** ***");
                Console.WriteLine("Could not find the window");
                throw new NullReferenceException("Could not find the window");
            }                

            return window;
        }

        public static void RemoveFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static void RemoveDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                RemoveDirectory(dir);
            }

            Directory.Delete(path, false);

        }

        public static bool IsFolderGit(string path)
        {
            string pathToDotGitFolder = Path.Combine(path, ConstantsList.dotGitFolder);
            return Directory.Exists(pathToDotGitFolder);
        }

        public static bool IsFolderMercurial(string path)
        {
            string pathToDotGitFolder = Path.Combine(path, ConstantsList.dotHgFolder);
            return Directory.Exists(pathToDotGitFolder);
        }
    }
}