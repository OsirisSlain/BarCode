namespace Barcode;

public class Ean8 : IBarcodeGenerator
{
	public string Name => "EAN-8";
	public string Description => "EAN-8 is an 8-digit barcode derived from EAN-13, designed for small packages where a full EAN-13 would not fit. It encodes 7 data digits and a trailing checksum digit. You can enter 7 digits to auto-calculate the checksum, or 8 to validate it.";

	private const string Guard = "101";
	private const string Middle = "01010";

	private static readonly string[] LeftEncoding = [
		"0001101", "0011001", "0010011", "0111101", "0100011",
		"0110001", "0101111", "0111011", "0110111", "0001011"
	];
	
	private static readonly string[] RightEncoding = [
		"1110010", "1100110", "1101100", "1000010", "1011100",
		"1001110", "1010000", "1000100", "1001000", "1110100"
	];

	public string? ValidateAndPad(string data)
	{
		var cleanData = data.Replace("-", "").Replace(" ", "");
		
		if (cleanData.Length == 7 && cleanData.All(char.IsAsciiDigit))
		{
			return cleanData + cleanData.CalculateGtinChecksum();
		}
		
		if (cleanData.Length == 8 && cleanData.IsValidGtin())
		{
			return cleanData;
		}
		
		return null;
	}

	public BarcodeData Generate(string validatedData)
	{
		var data = new BarcodeData();
		data.Add(SegmentType.QuietZone, "Left Quiet Zone", new string('0', 10));
		data.Add(SegmentType.Guard, "Start Guard", Guard);

		for (int i = 0; i < 4; i++)
		{
			int digit = validatedData[i] - '0';
			data.Add(SegmentType.Data, $"Left Data '{digit}'", LeftEncoding[digit]);
		}

		data.Add(SegmentType.Guard, "Middle Guard", Middle);

		for (int i = 4; i < 7; i++)
		{
			int digit = validatedData[i] - '0';
			data.Add(SegmentType.Data, $"Right Data '{digit}'", RightEncoding[digit]);
		}

		int checksumDigit = validatedData[7] - '0';
		data.Add(SegmentType.Checksum, $"Checksum '{checksumDigit}'", RightEncoding[checksumDigit]);

		data.Add(SegmentType.Guard, "End Guard", Guard);
		data.Add(SegmentType.QuietZone, "Right Quiet Zone", new string('0', 10));
		
		return data;
	}
}
