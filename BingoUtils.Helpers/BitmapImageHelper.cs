using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BingoUtils.Helpers
{
    public class BitmapImageHelper
    {
        /* From: https://social.msdn.microsoft.com/Forums/vstudio/en-US/5b2270cb-f182-4f5f-a6c6-c78dfe4e3230/how-to-dispose-a-systemwindowsmediaimagesource?forum=wpf
         * By: Alex Skalozub
        */
        /// <summary>
        /// Loads an image with CacheOption set to OnLoad
        /// </summary>
        /// <param name="source">The source for the image</param>
        /// <returns>The loaded BitmapImage</returns>
        public static ImageSource BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }
    }
}
