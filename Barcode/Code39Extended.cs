namespace Barcode;

public class Code39Extended : IBarcodeGenerator
{
	public string Name => "Code 39 (Extended)";
	public string Description => "Code 39 Extended uses pairs of standard Code 39 characters to represent all 128 ASCII characters. It demonstrates the concept of 'escape sequences' in older formats. For instance, a lowercase 'a' is encoded as '+A' behind the scenes.";

	private readonly Code39 _standardCode39 = new();

	private static readonly Dictionary<char, string> ExtendedMap = new()
	{
		{'\0', "%U"}, {'\x01', "$A"}, {'\x02', "$B"}, {'\x03', "$C"}, {'\x04', "$D"}, {'\x05', "$E"},
		{'\x06', "$F"}, {'\x07', "$G"}, {'\x08', "$H"}, {'\x09', "$I"}, {'\x0A', "$J"}, {'\x0B', "$K"},
		{'\x0C', "$L"}, {'\x0D', "$M"}, {'\x0E', "$N"}, {'\x0F', "$O"}, {'\x10', "$P"}, {'\x11', "$Q"},
		{'\x12', "$R"}, {'\x13', "$S"}, {'\x14', "$T"}, {'\x15', "$U"}, {'\x16', "$V"}, {'\x17', "$W"},
		{'\x18', "$X"}, {'\x19', "$Y"}, {'\x1A', "$Z"}, {'\x1B', "%A"}, {'\x1C', "%B"}, {'\x1D', "%C"},
		{'\x1E', "%D"}, {'\x1F', "%E"}, {' ', " "},   {'!', "/A"}, {'"', "/B"}, {'#', "/C"},
		{'$', "/D"}, {'%', "/E"}, {'&', "/F"}, {'\'', "/G"}, {'(', "/H"}, {')', "/I"},
		{'*', "/J"}, {'+', "/K"}, {',', "/L"}, {'-', "-"},   {'.', "."},   {'/', "/O"},
		{'0', "0"},   {'1', "1"},   {'2', "2"},   {'3', "3"},   {'4', "4"},   {'5', "5"},
		{'6', "6"},   {'7', "7"},   {'8', "8"},   {'9', "9"},   {':', "/Z"}, {';', "%F"},
		{'<', "%G"}, {'=', "%H"}, {'>', "%I"}, {'?', "%J"}, {'@', "%V"}, {'A', "A"},
		{'B', "B"},   {'C', "C"},   {'D', "D"},   {'E', "E"},   {'F', "F"},   {'G', "G"},
		{'H', "H"},   {'I', "I"},   {'J', "J"},   {'K', "K"},   {'L', "L"},   {'M', "M"},
		{'N', "N"},   {'O', "O"},   {'P', "P"},   {'Q', "Q"},   {'R', "R"},   {'S', "S"},
		{'T', "T"},   {'U', "U"},   {'V', "V"},   {'W', "W"},   {'X', "X"},   {'Y', "Y"},
		{'Z', "Z"},   {'[', "%K"}, {'\\', "%L"}, {']', "%M"}, {'^', "%N"}, {'_', "%O"},
		{'`', "%W"}, {'a', "+A"}, {'b', "+B"}, {'c', "+C"}, {'d', "+D"}, {'e', "+E"},
		{'f', "+F"}, {'g', "+G"}, {'h', "+H"}, {'i', "+I"}, {'j', "+J"}, {'k', "+K"},
		{'l', "+L"}, {'m', "+M"}, {'n', "+N"}, {'o', "+O"}, {'p', "+P"}, {'q', "+Q"},
		{'r', "+R"}, {'s', "+S"}, {'t', "+T"}, {'u', "+U"}, {'v', "+V"}, {'w', "+W"},
		{'x', "+X"}, {'y', "+Y"}, {'z', "+Z"}, {'{', "%P"}, {'|', "%Q"}, {'}', "%R"},
		{'~', "%S"}, {'\x7F', "%T"}
	};

	public string? ValidateAndPad(string data)
	{
		if (string.IsNullOrEmpty(data) || !data.All(ExtendedMap.ContainsKey))
		{
			return null;
		}

		string extended = "";
		foreach (char c in data)
		{
			extended += ExtendedMap[c];
		}

		return _standardCode39.ValidateAndPad(extended);
	}

	public BarcodeData Generate(string validatedData)
	{
		return _standardCode39.Generate(validatedData);
	}
}
