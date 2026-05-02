using System.Drawing.Imaging;

namespace Barcode;

public partial class MainForm : Form
{
	private readonly List<IBarcodeGenerator> _generators;
	private readonly ImageBuilder _imageBuilder;

	public MainForm()
	{
		InitializeComponent();
		
		_imageBuilder = new ImageBuilder();
		_generators = [
			new UpcA(),
			new Ean13(),
			new Ean8(),
			new Code39(),
			new Code39Extended(),
			new Code39Mod43(),
			new Code128(),
			new Interleaved2Of5(),
			new Codabar(),
			new Pharmacode(),
			new PostNet()
		];

		formatComboBox.DataSource = _generators;
		formatComboBox.DisplayMember = "Name";
		
		// Trigger initial population
		FormatComboBox_SelectedIndexChanged(this, EventArgs.Empty);
	}

	private void FormatComboBox_SelectedIndexChanged(object? sender, EventArgs e)
	{
		if (formatComboBox.SelectedItem is IBarcodeGenerator generator)
		{
			descriptionTextBox.Text = generator.Description;
		}
		GenerateBarcode(sender, e);
	}

	private void GenerateBarcode(object? sender, EventArgs e)
	{
		if (formatComboBox.SelectedItem is not IBarcodeGenerator generator) return;

		string input = dataTextBox.Text;
		
		if (string.IsNullOrWhiteSpace(input))
		{
			ClearBarcode();
			return;
		}

		string? validatedData = generator.ValidateAndPad(input);

		if (validatedData == null)
		{
			ClearBarcode();
			dataTextBox.BackColor = Color.LightPink;
			encodedDataLabel.Text = "Invalid data format or length for " + generator.Name;
			encodedDataLabel.ForeColor = Color.Red;
			return;
		}

		dataTextBox.BackColor = SystemColors.Window;
		encodedDataLabel.Text = $"Encoded Data: {validatedData}";
		encodedDataLabel.ForeColor = Color.DarkGreen;
		
		try
		{
			BarcodeData barcodeData = generator.Generate(validatedData);
			var oldImage = barcodePictureBox.Image;
			barcodePictureBox.Image = _imageBuilder.BuildBarcode(barcodeData);
			oldImage?.Dispose();
			
			var lines = new List<string>();
			foreach (var segment in barcodeData.Segments)
			{
				if (segment.Type != SegmentType.QuietZone)
				{
					lines.Add($"[{segment.Type.ToString().PadRight(8)}] {segment.Name.PadRight(25)} : {segment.Pattern}");
				}
			}
			breakdownTextBox.Text = string.Join(Environment.NewLine, lines);
		}
		catch
		{
			ClearBarcode();
			dataTextBox.BackColor = Color.LightPink;
		}
	}

	private void ClearBarcode()
	{
		barcodePictureBox.Image = null;
		dataTextBox.BackColor = SystemColors.Window;
		encodedDataLabel.Text = "";
		breakdownTextBox.Text = "";
	}

	private void SaveButton_Click(object? sender, EventArgs e)
	{
		if (barcodePictureBox.Image == null) return;

		using var dialog = new SaveFileDialog
		{
			Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp",
			Title = "Save Barcode",
			FileName = "barcode.png"
		};

		if (dialog.ShowDialog() == DialogResult.OK)
		{
			ImageFormat format = dialog.FilterIndex switch
			{
				2 => ImageFormat.Jpeg,
				3 => ImageFormat.Bmp,
				_ => ImageFormat.Png
			};
			barcodePictureBox.Image.Save(dialog.FileName, format);
		}
	}
}
