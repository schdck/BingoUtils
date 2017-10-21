using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BingoUtils.Helpers
{
    public static class PdfHelper
    {
        /// <summary>
        /// Adds an page to the PdfDocument and draws the control into it
        /// </summary>
        /// <param name="document">The document in which the picture should be added</param>
        /// <param name="element">The element to be drawed</param>
        /// <exception cref="System.ArgumentException" />
        /// <exception cref="System.ArgumentNullException" />
        /// <exception cref="PathTooLongException" />
        /// <exception cref="DirectoryNotFoundException" />
        /// <exception cref="IOException" />
        /// <exception cref="System.UnauthorizedAccessException" />
        /// <exception cref="System.ArgumentOutOfRangeException" />
        /// <exception cref="FileNotFoundException" />
        /// <exception cref="System.NotSupportedException" />
        /// <exception cref="System.Security.SecurityException" />
        /// <exception cref="System.InvalidOperationException" />
        /// <exception cref="System.Runtime.InteropServices.ExternalException" />
        public static void DrawPictureOfControlToPdf(PdfDocument document, FrameworkElement element)
        {
            string imagePath = Path.Combine(Path.GetTempPath(), "BingoTemp", "img.png");

            ExportControlAsPng(element, imagePath);

            using (FileStream stream = File.Open(imagePath, FileMode.Open))
            {
                DrawPictureToPdf(document, stream);
            }
        }

        // Based on a function found at: http://www.billrowell.com/2010/12/13/adding-an-image-to-a-pdf-document-using-c-and-pdfsharp/
        /// <summary>
        /// Exports a control as PNG
        /// </summary>
        /// <param name="element">The control to be exported</param>
        /// <param name="imagePath">The path where the image should be saved</param>
        /// <exception cref="System.ArgumentNullException" />
        /// <exception cref="System.NotSupportedException" />
        /// <exception cref="System.InvalidOperationException" />
        /// <exception cref="System.Runtime.InteropServices.ExternalException" />
        private static void ExportControlAsPng(FrameworkElement element, string imagePath)
        {
            RenderTargetBitmap renderer = new RenderTargetBitmap((int)element.ActualWidth, (int)element.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            renderer.Render(element);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderer));

            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);

                using (System.Drawing.Image image = System.Drawing.Image.FromStream(stream))
                {
                    image.Save(imagePath, ImageFormat.Png);
                }
            }
        }

        /// <summary>
        /// Adds a page to the PdfDocument and draws the image on int
        /// </summary>
        /// <param name="document">The document in which the picture should be added</param>
        /// <param name="stream">The stream of the image to be drawed</param>
        private static void DrawPictureToPdf(PdfDocument document, Stream stream)
        {
            PdfPage page = document.AddPage();

            page.Orientation = PdfSharp.PageOrientation.Landscape;

            using (XGraphics gfx = XGraphics.FromPdfPage(page))
            {
                using (XImage xImage = XImage.FromStream(stream))
                {
                    gfx.DrawImage(xImage, 30, 30);
                }
            }
        }
    }
}
