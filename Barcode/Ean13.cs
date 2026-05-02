namespace Barcode;

public class Ean13 : IBarcodeGenerator
{
	public string Name => "EAN-13";
	public string Description => "EAN-13 is the global standard for retail goods outside of North America. It encodes 12 data digits and a checksum. The 13th (first) digit is cleverly encoded by determining the parity (even/odd pattern) of the next 6 digits on the left side.";

	private const string Guard = "101";
	private const string Middle = "01010";

	private static readonly string[] LeftOdd = [
		"0001101", "0011001", "0010011", "0111101", "0100011",
		"0110001", "0101111", "0111011", "0110111", "0001011"
	];

	private static readonly string[] LeftEven = [
		"0100111", "0110011", "0011011", "0100001", "0011101",
		"0111001", "0000101", "0010001", "0001001", "0010111"
	];

	private static readonly string[] RightEncoding = [
		"1110010", "1100110", "1101100", "1000010", "1011100",
		"1001110", "1010000", "1000100", "1001000", "1110100"
	];

	private static readonly string[] ParityTable = [
		"OOOOOO", "OOEOEE", "OOEEOE", "OOEEEO", "OEOOEE",
		"OEEOOE", "OEEEOO", "OEOEOE", "OEOEEO", "OEEOEO"
	];

	public string? ValidateAndPad(string data)
	{
		var cleanData = data.Replace("-", "").Replace(" ", "");
		
		if (cleanData.Length == 12 && cleanData.All(char.IsAsciiDigit))
		{
			return cleanData + cleanData.CalculateGtinChecksum();
		}
		
		if (cleanData.Length == 13 && cleanData.IsValidGtin())
		{
			return cleanData;
		}
		
		return null;
	}

	public BarcodeData Generate(string validatedData)
	{
		var data = new BarcodeData();
		
		int firstDigit = validatedData[0] - '0';
		string parity = ParityTable[firstDigit];

		data.Add(SegmentType.QuietZone, "Left Quiet Zone", new string('0', 10));
		data.Add(SegmentType.Guard, "Start Guard", Guard);
		
		for (int i = 1; i <= 6; i++)
		{
			int digit = validatedData[i] - '0';
			string pattern = parity[i - 1] == 'O' ? LeftOdd[digit] : LeftEven[digit];
			data.Add(SegmentType.Data, $"Left Data '{digit}' (Parity: {parity[i - 1]})", pattern);
		}

		data.Add(SegmentType.Guard, "Middle Guard", Middle);

		for (int i = 7; i <= 11; i++)
		{
			int digit = validatedData[i] - '0';
			data.Add(SegmentType.Data, $"Right Data '{digit}'", RightEncoding[digit]);
		}
		
		int checksumDigit = validatedData[12] - '0';
		data.Add(SegmentType.Checksum, $"Checksum '{checksumDigit}'", RightEncoding[checksumDigit]);

		data.Add(SegmentType.Guard, "End Guard", Guard);
		data.Add(SegmentType.QuietZone, "Right Quiet Zone", new string('0', 10));

		return data;
	}
}
