using System.Drawing.Imaging;

namespace Barcode
{
    static class Program
    {
        static void Main()
        {
            var testUpc = UpcA.GenerateUpcA("045496891979");
            testUpc.Save("test.png", ImageFormat.Png);
        }
    }
}