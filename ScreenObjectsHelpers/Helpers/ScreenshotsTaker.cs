using System;
using TestStack.White;
using System.Drawing;
using System.Drawing.Imaging;

namespace ScreenObjectsHelpers.Helpers
{
    public class ScreenshotsTaker
    {
        // name of test should be passed to the method 
        // to include it to the name of screenshot file
        // e.g. TakeScreenShot(nameof(<name of test>))
        public static void TakeScreenShot(string nameOfTest)
        {            
            var prefix = "Test_";
            var timestamp = DateTime.Now.ToString("_MM.dd_HHmmss");
            var extension = ".jpg";

            var filename = prefix + nameOfTest + timestamp + extension;

            string path = Environment.ExpandEnvironmentVariables(@"");

            ScreenCapture sc = new ScreenCapture();
            // capture entire screen, and save it to a file
            Bitmap img = sc.CaptureScreenShot();
            img.Save(path + filename, ImageFormat.Jpeg);            
        }
    }
}