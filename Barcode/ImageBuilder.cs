using System.Drawing;

namespace Barcode;

public class ImageBuilder
{
	private static readonly double InversePhi = 2 / (1 + Math.Sqrt(5));

	public Color Background { get; set; } = Color.White;
	public int Scale { get; set; } = 4;

	private Color GetColorForSegment(SegmentType type) => type switch
	{
		SegmentType.Guard => Color.Red,
		SegmentType.Checksum => Color.Blue,
		SegmentType.QuietZone => Color.White,
		_ => Color.Black
	};

	public Bitmap BuildBarcode(BarcodeData barcodeData)
	{
		string fullCode = barcodeData.FullPattern;
		int width = fullCode.Length * Scale;
		int height = (int)(width * InversePhi);
		int margin = height / 9;

		var barcode = new Bitmap(width, height);
		using var graphics = Graphics.FromImage(barcode);
		
		graphics.Clear(Background);
		
		int currentX = 0;
		foreach (var segment in barcodeData.Segments)
		{
			if (segment.Type == SegmentType.QuietZone)
			{
				currentX += segment.Pattern.Length * Scale;
				continue;
			}

			using var brush = new SolidBrush(GetColorForSegment(segment.Type));
			
			foreach (char c in segment.Pattern)
			{
				if (c == '1')
				{
					// Full height
					graphics.FillRectangle(brush, currentX, margin, Scale, height - margin * 2);
				}
				else if (c == '2')
				{
					// Half height (anchored to bottom)
					int fullHeight = height - margin * 2;
					int halfHeight = fullHeight / 2;
					graphics.FillRectangle(brush, currentX, margin + halfHeight, Scale, halfHeight);
				}
				
				currentX += Scale;
			}
		}

		return barcode;
	}
}
