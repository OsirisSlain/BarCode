using System.Drawing.Imaging;

namespace Barcode
{
    static class Program
    {
        static void Main()
        {
            var testUpc = new UpcA().Generate("045496891979");
            testUpc.Save("test.png", ImageFormat.Png);
        }
    }
}