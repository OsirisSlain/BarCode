namespace Barcode;

public class UpcA : IBarcodeGenerator
{
	public string Name => "UPC-A";
	public string Description => "UPC-A is a 12-digit barcode used extensively in retail across North America. It encodes 11 data digits and a trailing checksum digit. You can enter 11 digits to auto-calculate the checksum, or 12 to validate it.";

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
		
		if (cleanData.Length == 11 && cleanData.All(char.IsAsciiDigit))
		{
			return cleanData + cleanData.CalculateGtinChecksum();
		}
		
		if (cleanData.Length == 12 && cleanData.IsValidGtin())
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

		for (int i = 0; i < 6; i++)
		{
			int digit = validatedData[i] - '0';
			data.Add(SegmentType.Data, $"Left Data '{digit}'", LeftEncoding[digit]);
		}

		data.Add(SegmentType.Guard, "Middle Guard", Middle);

		for (int i = 6; i < 11; i++)
		{
			int digit = validatedData[i] - '0';
			data.Add(SegmentType.Data, $"Right Data '{digit}'", RightEncoding[digit]);
		}

		int checksumDigit = validatedData[11] - '0';
		data.Add(SegmentType.Checksum, $"Checksum '{checksumDigit}'", RightEncoding[checksumDigit]);

		data.Add(SegmentType.Guard, "End Guard", Guard);
		data.Add(SegmentType.QuietZone, "Right Quiet Zone", new string('0', 10));
		
		return data;
	}
}
