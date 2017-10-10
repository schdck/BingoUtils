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

        private static void DrawPictureToPdf(PdfDocument doccument, Stream stream)
        {
            PdfPage page = doccument.AddPage();

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
