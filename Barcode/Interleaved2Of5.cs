namespace Barcode;

public class Interleaved2Of5 : IBarcodeGenerator
{
	public string Name => "Interleaved 2 of 5 (ITF)";
	public string Description => "Interleaved 2 of 5 is a continuous two-width barcode used primarily in distribution and warehouse industries. It is very unique educationally because it encodes pairs of digits simultaneously: the first digit is encoded in the 5 black bars, and the second digit is encoded in the 5 white spaces interleaved between them! Because of this paired encoding, it must always contain an even number of digits (we will pad it with a leading zero if you provide an odd number).";

	// 0: narrow, 1: wide
	private static readonly string[] Encoding = [
		"00110", // 0
		"10001", // 1
		"01001", // 2
		"11000", // 3
		"00101", // 4
		"10100", // 5
		"01100", // 6
		"00011", // 7
		"10010", // 8
		"01010"  // 9
	];

	public string? ValidateAndPad(string data)
	{
		var cleanData = data.Replace("-", "").Replace(" ", "");
		if (!cleanData.All(char.IsAsciiDigit) || cleanData.Length == 0)
			return null;

		// Pad with leading zero if odd number of digits
		if (cleanData.Length % 2 != 0)
			cleanData = "0" + cleanData;

		return cleanData;
	}

	public BarcodeData Generate(string validatedData)
	{
		var data = new BarcodeData();
		data.Add(SegmentType.QuietZone, "Left Quiet Zone", new string('0', 10));
		
		// Start pattern: 1010
		data.Add(SegmentType.Guard, "Start Guard", "1010");

		for (int i = 0; i < validatedData.Length; i += 2)
		{
			char c1 = validatedData[i];
			char c2 = validatedData[i + 1];
			string barEncoding = Encoding[c1 - '0'];
			string spaceEncoding = Encoding[c2 - '0'];

			string pattern = "";
			for (int j = 0; j < 5; j++)
			{
				pattern += barEncoding[j] == '1' ? "11" : "1";
				pattern += spaceEncoding[j] == '1' ? "00" : "0";
			}
			
			data.Add(SegmentType.Data, $"Pair '{c1}{c2}'", pattern);
		}

		// Stop pattern: 1101
		data.Add(SegmentType.Guard, "Stop Guard", "1101");
		data.Add(SegmentType.QuietZone, "Right Quiet Zone", new string('0', 10));
		
		return data;
	}
}
