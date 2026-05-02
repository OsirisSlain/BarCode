using System.Drawing.Drawing2D;

namespace Barcode;

public class SharpPictureBox : PictureBox
{
	protected override void OnPaint(PaintEventArgs pe)
	{
		pe.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
		pe.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
		base.OnPaint(pe);
	}
}
