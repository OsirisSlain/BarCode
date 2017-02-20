using System;
using System.Linq;

namespace Barcode
{
    class UpcA
    {
        private const string Guard = "101";
        private const string Middle = "01010";

        private static readonly string[] LeftEncoding = {
            "0001101", "0011001", "0010011", "0111101", "0100011",
            "0110001", "0101111", "0111011", "0110111", "0001011"
        };
        private static readonly string[] RightEncoding = {
            "1110010", "1100110", "1101100", "1000010", "1011100",
            "1001110", "1010000", "1000100", "1001000", "1110100"
        };

        public string Generate(string upc)
        {
            var cleanUpc = upc.Replace("-", "").Replace(" ", "");
            if (!cleanUpc.IsValidUpcA())
                Console.WriteLine("Warning: This is not a valid UPC-A!");

            var quiet = new string('0', 10);
            var leftDigits = cleanUpc.Take(6).Select(x => x - '0').GenerateBars(LeftEncoding);
            var rightDigits = cleanUpc.Skip(6).Select(x => x - '0').GenerateBars(RightEncoding);
            return quiet + Guard + leftDigits + Middle + rightDigits + Guard + quiet;
        }
    }
}