using System.Collections.Generic;
using System.Linq;

namespace Barcode
{
    static class Extensions
    {
        public static string GenerateBars(this IEnumerable<int> digits, string[] encoding)
        {
            return string.Join("", digits.Select(x => encoding[x]));
        }
    }
}