namespace Barcode;

public class PostNet : IBarcodeGenerator
{
	public string Name => "PostNet";
	public string Description => "PostNet was used by the USPS to route mail. Unlike most barcodes that vary the width of bars, PostNet varies the HEIGHT. It uses 'tall' and 'short' bars. A valid zip code must be 5, 9, or 11 digits, and it requires a modulo 10 checksum digit appended to the end to ensure the sum of all digits is a multiple of 10.";

	private static readonly string[] Encoding = [
		"101020202", // 0 (11000)
		"202020101", // 1 (00011)
		"202010201", // 2 (00101)
		"202010102", // 3 (00110)
		"201020201", // 4 (01001)
		"201020102", // 5 (01010)
		"201010202", // 6 (01100)
		"102020201", // 7 (10001)
		"102020102", // 8 (10010)
		"102010202"  // 9 (10100)
	];

	public string? ValidateAndPad(string data)
	{
		var cleanData = data.Replace("-", "").Replace(" ", "");
		if (!cleanData.All(char.IsAsciiDigit)) return null;

		if (cleanData.Length == 5 || cleanData.Length == 9 || cleanData.Length == 11)
		{
			int sum = cleanData.Sum(c => c - '0');
			int checksum = (10 - (sum % 10)) % 10;
			return cleanData + checksum;
		}
		
		if (cleanData.Length == 6 || cleanData.Length == 10 || cleanData.Length == 12)
		{
			int sum = cleanData.Take(cleanData.Length - 1).Sum(c => c - '0');
			int checksum = (10 - (sum % 10)) % 10;
			if (cleanData.Last() - '0' == checksum) return cleanData;
		}

		return null;
	}

	public BarcodeData Generate(string validatedData)
	{
		var data = new BarcodeData();
		data.Add(SegmentType.QuietZone, "Left Quiet Zone", new string('0', 10));
		data.Add(SegmentType.Guard, "Frame Bar (Start)", "10"); // Frame bar + space

		for (int i = 0; i < validatedData.Length; i++)
		{
			int digit = validatedData[i] - '0';
			string pattern = Encoding[digit];
			
			if (i < validatedData.Length - 1)
			{
				pattern += "0"; // Space between characters
			}
			
			if (i == validatedData.Length - 1) // Last digit is checksum
			{
				data.Add(SegmentType.Checksum, $"Checksum '{digit}'", pattern + "0");
			}
			else
			{
				data.Add(SegmentType.Data, $"Data '{digit}'", pattern);
			}
		}

		data.Add(SegmentType.Guard, "Frame Bar (Stop)", "1"); // Frame bar
		data.Add(SegmentType.QuietZone, "Right Quiet Zone", new string('0', 10));
		return data;
	}
}
