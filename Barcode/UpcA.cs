using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Barcode
{
    static class UpcA
    {
        static readonly double InversePhi = 2 / (1 + Math.Sqrt(5));

        private const string Guard = "101";
        private const string Middle = "01010";

        private static readonly string[] LeftEncoding = {
            "0001101", "0011001", "0010011", "0111101", "0100011",
            "0110001", "0101111", "0111011", "0110111", "0001011"
        };
        private static readonly string[] RightEncoding = {
            "1110010", "1100110", "1101100", "1000010", "1011100",
            "1001110", "1010000", "1000100", "1001000", "1110100"
        };

        public static Bitmap GenerateUpcA(string upc, int scale = 4, int quietSize = 12)
        {
            var cleanUpc = upc.Replace("-", "").Replace(" ", "");
            if (!cleanUpc.IsValidUpcA())
                Console.WriteLine("Warning: This is not a valid UPC-A!");

            var quiet = new string('0', quietSize);
            var leftDigits = cleanUpc.Take(6).Select(x => x - '0').GenerateBars(LeftEncoding);
            var rightDigits = cleanUpc.Skip(6).Select(x => x - '0').GenerateBars(RightEncoding);
            var fullCode = quiet + Guard + leftDigits + Middle + rightDigits + Guard + quiet;

            int width = fullCode.Length * scale;
            int height = (int)(width * InversePhi);
            int margin = (int)(quietSize * scale * InversePhi);

            var bars = new Bitmap(width, height);
            for (int x = 0; x < width; x++)
            {
                var color = fullCode[x / scale] == '0' ? Color.White : Color.Black;
                for (int y = 0; y < height; y++)
                {
                    if (y < margin || y > (height - margin))
                        bars.SetPixel(x, y, Color.White);
                    else
                        bars.SetPixel(x, y, color);
                }
            }
            return bars;
        }

        private static string GenerateBars(this IEnumerable<int> digits, string[] encoding)
        {
            return string.Join("", digits.Select(x => encoding[x]));
        }
    }
}