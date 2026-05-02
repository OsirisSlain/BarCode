namespace Barcode;

public enum SegmentType
{
	QuietZone,
	Guard,
	Data,
	Checksum
}

public record BarcodeSegment(SegmentType Type, string Name, string Pattern);

public class BarcodeData
{
	public List<BarcodeSegment> Segments { get; } = new();

	public string FullPattern => string.Join("", Segments.Select(s => s.Pattern));
	
	public void Add(SegmentType type, string name, string pattern)
	{
		Segments.Add(new BarcodeSegment(type, name, pattern));
	}
}

public interface IBarcodeGenerator
{
	string Name { get; }
	string Description { get; }
	
	/// <summary>
	/// Normalizes the input, auto-calculates checksum if needed, and validates.
	/// Returns the full validated string, or null if the input is completely invalid.
	/// </summary>
	string? ValidateAndPad(string data);
	
	/// <summary>
	/// Generates the segmented representation from the validated data.
	/// </summary>
	BarcodeData Generate(string validatedData);
}
