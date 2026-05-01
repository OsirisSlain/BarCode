namespace Barcode;

public static class Extensions
{
	public static string GenerateBars(this IEnumerable<int> digits, string[] encoding) =>
		string.Join("", digits.Select(x => encoding[x]));
}
