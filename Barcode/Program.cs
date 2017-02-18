using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Barcode
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        static Bitmap GenerateUpcA(string upc)
        {
            var cleanUpc = upc.Replace("-", "").Replace(" ", "");
            var digits = cleanUpc.Select(Convert.ToInt32);
            var bars = new Bitmap(135, 84);

            var quietZone = "00000000000000000000";
            var start = "101";
            var middle = "01010";

            return bars;
        }
    }
}
