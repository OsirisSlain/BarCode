using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace Barcode
{
    static class Program
    {
        static string quiet = "00000000000000000000";
        static string guard = "101";
        static string middle = "01010";
        static string[] leftEncoding = new[]
        {
            "0001101", "0011001", "0010011", "0111101", "0100011",
            "0110001", "0101111", "0111011", "0110111", "0001011"
        };
        static string[] rightEncoding = new[]
        {
            "1110010", "1100110", "1101100", "1000010", "1011100",
            "1001110", "1010000", "1000100", "1001000", "1110100"
        };
        static string GenerateBarString(this IEnumerable<int> digits, string[] encoding)
        {
            return string.Join("", digits.Select(x => encoding[x]));
        }

        static void Main(string[] args)
        {
            var wat = GenerateUpcA("123456789012");
            wat.Save("test.png", ImageFormat.Png);
        }

        static Bitmap GenerateUpcA(string upc)
        {
            var cleanUpc = upc.Replace("-", "").Replace(" ", "");
            var leftDigits = cleanUpc.Take(6).Select(x => x - '0').GenerateBarString(leftEncoding);
            var rightDigits = cleanUpc.Skip(6).Select(x => x - '0').GenerateBarString(rightEncoding);
            var fullCode = quiet + guard + leftDigits + middle + rightDigits + guard + quiet;

            var bars = new Bitmap(135, 84);
            for (int x = 0; x < bars.Width; x++)
            {
                var color = fullCode[x] == '0' ? Color.White : Color.Black;
                for (int y = 0; y < bars.Height; y++)
                {
                    bars.SetPixel(x, y, color);
                }
            }
            return bars;
        }
    }
}
