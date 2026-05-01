using System.Drawing;

namespace Barcode;

public class ImageBuilder
{
	private static readonly double InversePhi = 2 / (1 + Math.Sqrt(5));

	public Color Background { get; set; } = Color.White;
	public Color BarColor { get; set; } = Color.Black;
	public int Scale { get; set; } = 4;

	public Bitmap BuildBarcode(string fullCode)
	{
		int width = fullCode.Length * Scale;
		int height = (int)(width * InversePhi);
		int margin = height / 9;

		var barcode = new Bitmap(width, height);
		using var graphics = Graphics.FromImage(barcode);
		
		graphics.Clear(Background);
		using var brush = new SolidBrush(BarColor);
		
		for (int i = 0; i < fullCode.Length; i++)
		{
			if (fullCode[i] == '1')
			{
				graphics.FillRectangle(brush, i * Scale, margin, Scale, height - margin * 2);
			}
		}

		return barcode;
	}
}
