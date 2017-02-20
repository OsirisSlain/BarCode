using System;
using System.Drawing;

namespace Barcode
{
    public class ImageBuilder
    {
        static readonly double InversePhi = 2 / (1 + Math.Sqrt(5));

        public Color Background { get; set; } = Color.White;
        public Color BarColor { get; set; } = Color.Black;
        public int Scale { get; set; } = 4;

        public Bitmap BuildBarcode(string fullCode)
        {
            int width = fullCode.Length * Scale;
            int height = (int) (width * InversePhi);
            int margin = height / 9;

            var barcode = new Bitmap(width, height);
            for (int x = 0; x < width; x++)
            {
                var color = fullCode[x / Scale] == '0' ? Background : BarColor;
                for (int y = 0; y < height; y++)
                {
                    if (y < margin || y > (height - margin))
                        barcode.SetPixel(x, y, Background);
                    else
                        barcode.SetPixel(x, y, color);
                }
            }
            return barcode;
        }
    }
}
