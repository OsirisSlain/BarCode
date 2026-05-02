namespace Barcode;

public class Codabar : IBarcodeGenerator
{
	public string Name => "Codabar";
	public string Description => "Codabar is an older barcode format used by libraries, blood banks, and FedEx. It encodes digits and a few special characters (- $ : / . +). It is interesting educationally because it requires arbitrary start and stop characters chosen from A, B, C, or D. This allowed users to convey additional meaning beyond just the data (e.g., 'A...B' might indicate a book, while 'C...D' might indicate a patron ID). We will default to A and B if you don't type any.";

	private static readonly Dictionary<char, string> EncodingMap = new()
	{
		{'0', "0000011"}, {'1', "0000110"}, {'2', "0001001"}, {'3', "1100000"},
		{'4', "0010010"}, {'5', "1000010"}, {'6', "0100001"}, {'7', "0100100"},
		{'8', "0110000"}, {'9', "1001000"}, {'-', "0001100"}, {'$', "0011000"},
		{':', "1000101"}, {'/', "1010001"}, {'.', "1010100"}, {'+', "0010110"},
		{'A', "0011010"}, {'B', "0101001"}, {'C', "0001011"}, {'D', "0001110"},
		{'T', "0011010"}, {'N', "0101001"}, {'*', "0001011"}, {'E', "0001110"} // Aliases for ABCD
	};

	public string? ValidateAndPad(string data)
	{
		string upper = data.ToUpperInvariant();
		if (string.IsNullOrEmpty(upper)) return null;

		// Default start/stop if none provided
		if (upper.All(c => "0123456789-$:/.+".Contains(c)))
		{
			upper = $"A{upper}B";
		}
		
		char start = upper[0];
		char stop = upper[^1];
		
		if (!"ABCDTN*E".Contains(start) || !"ABCDTN*E".Contains(stop)) return null;
		
		for (int i = 1; i < upper.Length - 1; i++)
		{
			if (!"0123456789-$:/.+".Contains(upper[i])) return null;
		}

		return upper;
	}

	public BarcodeData Generate(string validatedData)
	{
		var data = new BarcodeData();
		data.Add(SegmentType.QuietZone, "Left Quiet Zone", new string('0', 10));

		for (int i = 0; i < validatedData.Length; i++)
		{
			char c = validatedData[i];
			string widthsMap = EncodingMap[c];
			string pattern = "";
			
			for (int j = 0; j < 7; j++)
			{
				bool isBar = j % 2 == 0;
				int width = widthsMap[j] == '1' ? 2 : 1;
				pattern += new string(isBar ? '1' : '0', width);
			}
			
			if (i < validatedData.Length - 1)
			{
				pattern += "0"; // Inter-character gap (narrow space)
			}
			
			SegmentType type = (i == 0 || i == validatedData.Length - 1) ? SegmentType.Guard : SegmentType.Data;
			string name = (i == 0 || i == validatedData.Length - 1) ? $"Guard '{c}'" : $"Data '{c}'";
			
			data.Add(type, name, pattern);
		}

		data.Add(SegmentType.QuietZone, "Right Quiet Zone", new string('0', 10));
		return data;
	}
}
