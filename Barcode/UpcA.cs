using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Barcode
{
    static class UpcA
    {
        private const string Quiet = "000000000000";
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

        public static Bitmap GenerateUpcA(string upc, int scale = 4)
        {
            var cleanUpc = upc.Replace("-", "").Replace(" ", "");
            var leftDigits = cleanUpc.Take(6).Select(x => x - '0').GenerateBarString(LeftEncoding);
            var rightDigits = cleanUpc.Skip(6).Select(x => x - '0').GenerateBarString(RightEncoding);
            var fullCode = Quiet + Guard + leftDigits + Middle + rightDigits + Guard + Quiet;

            int width = fullCode.Length * scale;
            int height = (int)(width * 0.618);
            int margin = height / 7;
            var bars = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                var color = fullCode[x/scale] == '0' ? Color.White : Color.Black;
                for (int y = 0; y < height; y++)
                {
                    if (y < margin || y > (height - margin))
                        bars.SetPixel(x,y,Color.White);
                    else
                        bars.SetPixel(x, y, color);
                }
            }

            return bars;
        }

        private static string GenerateBarString(this IEnumerable<int> digits, string[] encoding)
        {
            return string.Join("", digits.Select(x => encoding[x]));
        }
    }
}