using System.Drawing.Imaging;
using Barcode;

var binaryBars = new UpcA().Generate("045496891978");
var image = new ImageBuilder().BuildBarcode(binaryBars);

image.Save("test.png", ImageFormat.Png);
