namespace Barcode;

public class Pharmacode : IBarcodeGenerator
{
	public string Name => "Pharmacode";
	public string Description => "Pharmacode is used in the pharmaceutical industry. Unlike most others, it does NOT encode data character-by-character. Instead, it encodes a single integer (between 3 and 131070) in binary, using only the width of the bars (spaces are ignored). It also lacks start or stop characters!";

	public string? ValidateAndPad(string data)
	{
		if (int.TryParse(data, out int value) && value >= 3 && value <= 131070)
		{
			return value.ToString();
		}
		return null;
	}

	public BarcodeData Generate(string validatedData)
	{
		int n = int.Parse(validatedData);
		int originalN = n;
		List<string> bars = new();
		
		while (n > 0)
		{
			if (n % 2 == 0)
			{
				bars.Insert(0, "11"); // Wide
				n = (n - 2) / 2;
			}
			else
			{
				bars.Insert(0, "1"); // Narrow
				n = (n - 1) / 2;
			}
		}

		var data = new BarcodeData();
		data.Add(SegmentType.QuietZone, "Left Quiet Zone", new string('0', 10));
		data.Add(SegmentType.Data, $"Value '{originalN}'", string.Join("0", bars));
		data.Add(SegmentType.QuietZone, "Right Quiet Zone", new string('0', 10));
		
		return data;
	}
}
