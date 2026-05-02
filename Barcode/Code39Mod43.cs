namespace Barcode;

public class Code39Mod43 : IBarcodeGenerator
{
	public string Name => "Code 39 (Mod 43)";
	public string Description => "This is a variant of Code 39 that includes an optional Modulo 43 checksum character for higher reliability. The checksum is calculated by summing the values (0-42) of all characters in the data, then taking the remainder when divided by 43. This character is appended just before the final '*' stop character.";

	private static readonly Dictionary<char, string> EncodingMap = new()
	{
		{'0', "111221211"}, {'1', "211211112"}, {'2', "112211112"}, {'3', "212211111"},
		{'4', "111221112"}, {'5', "211221111"}, {'6', "112221111"}, {'7', "111211212"},
		{'8', "211211211"}, {'9', "112211211"}, {'A', "211112112"}, {'B', "112112112"},
		{'C', "212112111"}, {'D', "111122112"}, {'E', "211122111"}, {'F', "112122111"},
		{'G', "111112212"}, {'H', "211112211"}, {'I', "112112211"}, {'J', "111122211"},
		{'K', "211111122"}, {'L', "112111122"}, {'M', "212111121"}, {'N', "111121122"},
		{'O', "211121121"}, {'P', "112121121"}, {'Q', "111111222"}, {'R', "211111221"},
		{'S', "112111221"}, {'T', "111121221"}, {'U', "221111112"}, {'V', "122111112"},
		{'W', "222111111"}, {'X', "121121112"}, {'Y', "221121111"}, {'Z', "122121111"},
		{'-', "121111212"}, {'.', "221111211"}, {' ', "122111211"}, {'*', "121121211"},
		{'$', "121212111"}, {'/', "121211121"}, {'+', "121112121"}, {'%', "111212121"}
	};

	private static readonly char[] ValueMap = 
	[
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
		'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
		'U', 'V', 'W', 'X', 'Y', 'Z', '-', '.', ' ', '$',
		'/', '+', '%'
	];

	public string? ValidateAndPad(string data)
	{
		string upper = data.ToUpperInvariant();
		if (string.IsNullOrEmpty(upper) || upper.Any(c => !EncodingMap.ContainsKey(c) || c == '*'))
		{
			return null;
		}

		int sum = 0;
		foreach (char c in upper)
		{
			sum += Array.IndexOf(ValueMap, c);
		}
		
		char checksumChar = ValueMap[sum % 43];
		
		return $"*{upper}{checksumChar}*";
	}

	public BarcodeData Generate(string validatedData)
	{
		var data = new BarcodeData();
		data.Add(SegmentType.QuietZone, "Left Quiet Zone", new string('0', 10));

		for (int i = 0; i < validatedData.Length; i++)
		{
			char c = validatedData[i];
			string widths = EncodingMap[c];
			string pattern = "";
			
			for (int j = 0; j < 9; j++)
			{
				bool isBar = j % 2 == 0;
				int width = widths[j] == '1' ? 1 : 2; 
				pattern += new string(isBar ? '1' : '0', width);
			}
			
			if (i < validatedData.Length - 1)
			{
				pattern += "0"; // Inter-character gap
			}
			
			SegmentType type = SegmentType.Data;
			string name = $"Data '{c}'";
			
			if (c == '*' && (i == 0 || i == validatedData.Length - 1))
			{
				type = SegmentType.Guard;
				name = i == 0 ? "Start Guard '*'" : "Stop Guard '*'";
			}
			else if (i == validatedData.Length - 2) // second to last is checksum
			{
				type = SegmentType.Checksum;
				name = $"Checksum '{c}'";
			}
			
			data.Add(type, name, pattern);
		}
		
		data.Add(SegmentType.QuietZone, "Right Quiet Zone", new string('0', 10));
		return data;
	}
}
