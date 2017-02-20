using System.Drawing.Imaging;

namespace Barcode
{
    static class Program
    {
        static void Main()
        {
            var binaryBars = new UpcA().Generate("045496891978");
            var image = new ImageBuilder().BuildBarcode(binaryBars);
            image.Save("test.png", ImageFormat.Png);
        }
    }
}