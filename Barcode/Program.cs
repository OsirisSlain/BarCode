using System.Drawing.Imaging;

namespace Barcode
{
    static class Program
    {
        static void Main(string[] args)
        {
            var wat = UpcA.GenerateUpcA("045496891978");
            wat.Save("test.png", ImageFormat.Png);
        }
    }
}