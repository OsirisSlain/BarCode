namespace Barcode;

public static class Validators
{
	public static bool IsValidUpcA(this string upca) => 
		upca.Length == 12 && upca.IsValidGtin();

	public static bool IsValidGtin(this string gtin)
	{
		if (!gtin.All(char.IsAsciiDigit)) return false;

		var digits = gtin.Select(x => x - '0').Reverse().ToList();
		var even = digits.Where((_, i) => i % 2 == 0).Sum() * 3;
		var odd = digits.Where((_, i) => i % 2 != 0).Sum();
		
		return (even + odd) % 10 == 0;
	}
}
