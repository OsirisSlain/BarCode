namespace Barcode;

partial class MainForm
{
	private System.ComponentModel.IContainer components = null;
	private ComboBox formatComboBox;
	private TextBox dataTextBox;
	private SharpPictureBox barcodePictureBox;
	private Button saveButton;
	private Label formatLabel;
	private Label dataLabel;
	private TextBox descriptionTextBox;
	private Label encodedDataLabel;
	private TextBox breakdownTextBox;

	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.formatComboBox = new ComboBox();
		this.dataTextBox = new TextBox();
		this.barcodePictureBox = new SharpPictureBox();
		this.saveButton = new Button();
		this.formatLabel = new Label();
		this.dataLabel = new Label();
		this.descriptionTextBox = new TextBox();
		this.encodedDataLabel = new Label();
		this.breakdownTextBox = new TextBox();
		
		this.SuspendLayout();
		
		// formatLabel
		this.formatLabel.AutoSize = true;
		this.formatLabel.Location = new Point(12, 15);
		this.formatLabel.Name = "formatLabel";
		this.formatLabel.Size = new Size(48, 15);
		this.formatLabel.TabIndex = 0;
		this.formatLabel.Text = "Format:";
		
		// formatComboBox
		this.formatComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
		this.formatComboBox.FormattingEnabled = true;
		this.formatComboBox.Location = new Point(66, 12);
		this.formatComboBox.Name = "formatComboBox";
		this.formatComboBox.Size = new Size(121, 23);
		this.formatComboBox.TabIndex = 1;
		this.formatComboBox.SelectedIndexChanged += new EventHandler(this.FormatComboBox_SelectedIndexChanged);
		
		// dataLabel
		this.dataLabel.AutoSize = true;
		this.dataLabel.Location = new Point(203, 15);
		this.dataLabel.Name = "dataLabel";
		this.dataLabel.Size = new Size(34, 15);
		this.dataLabel.TabIndex = 2;
		this.dataLabel.Text = "Data:";
		
		// dataTextBox
		this.dataTextBox.Location = new Point(243, 12);
		this.dataTextBox.Name = "dataTextBox";
		this.dataTextBox.Size = new Size(229, 23);
		this.dataTextBox.TabIndex = 3;
		this.dataTextBox.TextChanged += new EventHandler(this.GenerateBarcode);
		
		// saveButton
		this.saveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		this.saveButton.Location = new Point(488, 11);
		this.saveButton.Name = "saveButton";
		this.saveButton.Size = new Size(85, 24);
		this.saveButton.TabIndex = 4;
		this.saveButton.Text = "Save...";
		this.saveButton.UseVisualStyleBackColor = true;
		this.saveButton.Click += new EventHandler(this.SaveButton_Click);
		
		// descriptionTextBox
		this.descriptionTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		this.descriptionTextBox.Location = new Point(12, 45);
		this.descriptionTextBox.Multiline = true;
		this.descriptionTextBox.ReadOnly = true;
		this.descriptionTextBox.ScrollBars = ScrollBars.Vertical;
		this.descriptionTextBox.Name = "descriptionTextBox";
		this.descriptionTextBox.Size = new Size(561, 80);
		this.descriptionTextBox.TabIndex = 5;
		this.descriptionTextBox.Text = "Description";
		
		// encodedDataLabel
		this.encodedDataLabel.AutoSize = true;
		this.encodedDataLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
		this.encodedDataLabel.Location = new Point(12, 130);
		this.encodedDataLabel.Name = "encodedDataLabel";
		this.encodedDataLabel.Size = new Size(0, 15);
		this.encodedDataLabel.TabIndex = 6;
		
		// barcodePictureBox
		this.barcodePictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		this.barcodePictureBox.BackColor = SystemColors.ControlLight;
		this.barcodePictureBox.Location = new Point(12, 155);
		this.barcodePictureBox.Name = "barcodePictureBox";
		this.barcodePictureBox.Size = new Size(561, 253);
		this.barcodePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
		this.barcodePictureBox.TabIndex = 7;
		this.barcodePictureBox.TabStop = false;
		
		// breakdownTextBox
		this.breakdownTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		this.breakdownTextBox.Location = new Point(12, 415);
		this.breakdownTextBox.Multiline = true;
		this.breakdownTextBox.ReadOnly = true;
		this.breakdownTextBox.ScrollBars = ScrollBars.Vertical;
		this.breakdownTextBox.Name = "breakdownTextBox";
		this.breakdownTextBox.Size = new Size(561, 120);
		this.breakdownTextBox.TabIndex = 8;
		this.breakdownTextBox.Text = "";
		this.breakdownTextBox.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
		
		// MainForm
		this.AutoScaleDimensions = new SizeF(7F, 15F);
		this.AutoScaleMode = AutoScaleMode.Font;
		this.ClientSize = new Size(585, 550);
		this.Controls.Add(this.breakdownTextBox);
		this.Controls.Add(this.barcodePictureBox);
		this.Controls.Add(this.encodedDataLabel);
		this.Controls.Add(this.descriptionTextBox);
		this.Controls.Add(this.saveButton);
		this.Controls.Add(this.dataTextBox);
		this.Controls.Add(this.dataLabel);
		this.Controls.Add(this.formatComboBox);
		this.Controls.Add(this.formatLabel);
		this.MinimumSize = new Size(500, 360);
		this.Name = "MainForm";
		this.Text = "Educational Barcode Generator";
		
		this.ResumeLayout(false);
		this.PerformLayout();
	}
}
