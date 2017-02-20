using System.Linq;
using System.Text.RegularExpressions;

namespace Barcode
{
    static class Validators
    {
        public static bool IsValidUpcA(this string upca)
        {
            if (upca.Length != 12) return false;
            return IsValidGtin(upca);
        }

        public static bool IsValidGtin(this string gtin)
        {
            if (!new Regex(@"^\d+$").IsMatch(gtin)) return false;
            var digits = gtin.Select(x => x - '0').Reverse();
            var even = digits.Where((x, i) => i % 2 == 0).Sum() * 3;
            var odd = digits.Where((x, i) => i % 2 != 0).Sum();
            return (even + odd) % 10 == 0;
        }
    }
}
