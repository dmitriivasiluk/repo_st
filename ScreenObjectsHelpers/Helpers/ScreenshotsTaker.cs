using System;
using TestStack.White;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace ScreenObjectsHelpers.Helpers
{
    public class ScreenshotsTaker
    {
        // name of test should be passed to the method 
        // to include it to the name of screenshot file
        // e.g. TakeScreenShot(nameof(<name of test>))
        // or a frame name from StackTrace
        // ScreenshotsTaker.TakeScreenShot(new StackTrace().GetFrame(0).GetMethod().Name);
        public static void TakeScreenShot(string nameOfTest)
        {
            var prefix = "Test_";
            var timestamp = DateTime.Now.ToString("_MM.dd_HHmmss");
            var random = new Random().Next().ToString();
            var extension = ".jpg";

            var filename = prefix + nameOfTest + timestamp + random + extension;

            string path = Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\");

            ScreenCapture sc = new ScreenCapture();
            // capture entire screen, and save it to a file
            Bitmap img = sc.CaptureScreenShot();
            img.Save(path + filename, ImageFormat.Jpeg);            
        }
    }
}