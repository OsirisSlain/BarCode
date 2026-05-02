namespace Barcode;

public class Code128 : IBarcodeGenerator
{
	public string Name => "Code 128";
	public string Description => "Code 128 is a high-density modern barcode. It is incredibly educational because it uses three distinct 'Code Sets' (A, B, C) to compress data. Code Set C compresses numeric data by encoding pairs of digits (00-99) into a single symbol! This generator will automatically switch between Code Set B (for text) and Code Set C (for numbers) to make the barcode as small as possible.";

	// (Value, Pattern)
	private static readonly string[] Encoding = [
		"212222", "222122", "222221", "121223", "121322", "131222", "122213", "122312", "132212", "221213",
		"221312", "231212", "112232", "122132", "122231", "113222", "123122", "123221", "223211", "221132",
		"221231", "213212", "223112", "312131", "311222", "321122", "321221", "312212", "322112", "322211",
		"212123", "212321", "232121", "111323", "131123", "131321", "112313", "132113", "132311", "211313",
		"231113", "231311", "112133", "112331", "132131", "113123", "113321", "133121", "313121", "211331",
		"231131", "213113", "213311", "213131", "311123", "311321", "331121", "312113", "312311", "332111",
		"314111", "221411", "431111", "111224", "111422", "121124", "121421", "141122", "141221", "112214",
		"112412", "122114", "122411", "142112", "142211", "241211", "221114", "413111", "241112", "134111",
		"111242", "121142", "121241", "114212", "124112", "124211", "411212", "421112", "421211", "212141",
		"214121", "412121", "111143", "111341", "131141", "114113", "114311", "411113", "411311", "113141",
		"114131", "311141", "411131", "211412", "211214", "211232", "233111"
	];

	public string? ValidateAndPad(string data)
	{
		if (string.IsNullOrEmpty(data) || !data.All(c => c >= 32 && c <= 126)) return null;
		return data;
	}

	public BarcodeData Generate(string validatedData)
	{
		var data = new BarcodeData();
		data.Add(SegmentType.QuietZone, "Left Quiet Zone", new string('0', 10));

		// Optimization logic: Try to use Code C (pairs of digits) if we have 4 or more digits in a row.
		List<int> values = new();
		List<string> segmentNames = new();
		
		int i = 0;
		bool inCodeC = false;

		// Start character
		if (StartsWith4Digits(validatedData, 0))
		{
			values.Add(105); // Start C
			segmentNames.Add("Start Code C");
			inCodeC = true;
		}
		else
		{
			values.Add(104); // Start B
			segmentNames.Add("Start Code B");
		}

		while (i < validatedData.Length)
		{
			if (inCodeC)
			{
				if (i + 1 < validatedData.Length && char.IsDigit(validatedData[i]) && char.IsDigit(validatedData[i+1]))
				{
					int pair = (validatedData[i] - '0') * 10 + (validatedData[i+1] - '0');
					values.Add(pair);
					segmentNames.Add($"Data '{validatedData[i]}{validatedData[i+1]}' (Code C)");
					i += 2;
				}
				else
				{
					values.Add(100); // Switch to Code B
					segmentNames.Add("Switch to Code B");
					inCodeC = false;
				}
			}
			else
			{
				if (StartsWith4Digits(validatedData, i))
				{
					values.Add(99); // Switch to Code C
					segmentNames.Add("Switch to Code C");
					inCodeC = true;
				}
				else
				{
					char c = validatedData[i];
					values.Add(c - 32);
					segmentNames.Add($"Data '{c}' (Code B)");
					i++;
				}
			}
		}

		// Calculate Checksum
		int checksum = values[0];
		for (int j = 1; j < values.Count; j++)
		{
			checksum += values[j] * j;
		}
		checksum %= 103;
		
		values.Add(checksum);
		segmentNames.Add($"Checksum '{checksum}'");

		values.Add(106); // Stop Character
		segmentNames.Add("Stop Guard");

		// Build segments
		for (int j = 0; j < values.Count; j++)
		{
			int val = values[j];
			string widths = Encoding[val];
			string pattern = "";
			
			for (int k = 0; k < widths.Length; k++)
			{
				bool isBar = k % 2 == 0;
				int width = widths[k] - '0';
				pattern += new string(isBar ? '1' : '0', width);
			}

			if (val == 106)
			{
				pattern += "11"; // The final 2-width bar of the stop character
			}

			SegmentType type = SegmentType.Data;
			if (j == 0 || j == values.Count - 1) type = SegmentType.Guard;
			else if (j == values.Count - 2) type = SegmentType.Checksum;
			else if (val == 99 || val == 100) type = SegmentType.Guard; // Make code switch visually distinct

			data.Add(type, segmentNames[j], pattern);
		}

		data.Add(SegmentType.QuietZone, "Right Quiet Zone", new string('0', 10));
		return data;
	}

	private bool StartsWith4Digits(string str, int index)
	{
		if (index + 3 >= str.Length) return false;
		for (int i = 0; i < 4; i++)
		{
			if (!char.IsDigit(str[index + i])) return false;
		}
		return true;
	}
}
