namespace ClearMine.Framework.Visual
{
    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Security;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    [Export(typeof(IVisualShoot))]
    public class VisualShooter : IVisualShoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="imageFilePath"></param>
        [SecurityCritical]
        public void SaveSnapshoot(FrameworkElement element, string imageFilePath)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            if (File.Exists(imageFilePath))
            {
                throw new InvalidOperationException(ResourceHelper.FindText("TargetAlreadyExists", imageFilePath));
            }

            var targetBitmap = new RenderTargetBitmap((int)element.ActualWidth, (int)element.ActualHeight, 96d, 96d, PixelFormats.Default);
            targetBitmap.Render(element);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(targetBitmap));

            var folder = Path.GetDirectoryName(imageFilePath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            using (var fs = File.Open(imageFilePath, FileMode.OpenOrCreate))
            {
                encoder.Save(fs);
            }

            Trace.TraceInformation(ResourceHelper.FindText("ScreenShotSavedTo"), imageFilePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public string SaveSnapshoot(FrameworkElement element)
        {
            var fileName = DateTime.Now.ToString(Settings.Default.ScreenShotFileTimeFormat, CultureInfo.InvariantCulture) + ".png";
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Settings.Default.ScreenShotFolder;
            fileName = Path.Combine(folder, fileName);

            SaveSnapshoot(element, fileName);

            return fileName;
        }
    }
}
