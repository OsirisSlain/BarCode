using System.Drawing;

namespace Barcode
{
    interface IBarcode
    {
         Bitmap Generate(string code, int scale, int quietSize);
    }
}