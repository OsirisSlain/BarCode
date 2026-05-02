namespace Barcode;

public static class Validators
{
	public static bool IsValidUpcA(this string upca) => 
		upca.Length == 12 && upca.IsValidGtin();

	public static bool IsValidGtin(this string gtin)
	{
		if (string.IsNullOrEmpty(gtin) || !gtin.All(char.IsAsciiDigit)) return false;

		var digits = gtin.Select(x => x - '0').Reverse().ToList();
		
		// Fix: Standard GTIN calculation multiplies the checksum (index 0) and every other alternating digit by 1,
		// and the rest by 3. The original code incorrectly multiplied index 0 by 3.
		var sum = digits.Select((d, i) => d * (i % 2 == 0 ? 1 : 3)).Sum();
		
		return sum % 10 == 0;
	}

	public static char CalculateGtinChecksum(this string dataWithoutChecksum)
	{
		if (string.IsNullOrEmpty(dataWithoutChecksum) || !dataWithoutChecksum.All(char.IsAsciiDigit)) 
			throw new ArgumentException("Data must be numeric.");
		
		var digits = dataWithoutChecksum.Select(x => x - '0').Reverse().ToList();
		
		// For the data portion (no checksum), the right-most digit is multiplied by 3, the next by 1, etc.
		var sum = digits.Select((d, i) => d * (i % 2 == 0 ? 3 : 1)).Sum();
		var checksum = (10 - (sum % 10)) % 10;
		
		return (char)(checksum + '0');
	}
}
