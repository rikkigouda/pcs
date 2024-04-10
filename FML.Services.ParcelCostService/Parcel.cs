public class Parcel
{
	private Parcel()
		: base()
	{ }

	public Parcel(decimal width, decimal height, decimal length, decimal weight)
		: this()
	{
		this.Width = width;

		this.Height = height;

		this.Length = length;

		this.Weight = weight;

		var dimensions = new[]
		{
			Width,
			Height,
			Length
		};

		if (dimensions.Any((value) => value <= 0))
		{
			throw new InvalidParcelDimensionsException(dimensions);
		}
	}

	public decimal Width { get; set; }

	public decimal Height { get; set; }

	public decimal Length { get; set; }

	public decimal Weight { get; set; }
}
