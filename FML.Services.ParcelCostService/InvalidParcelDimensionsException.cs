[Serializable]
public class InvalidParcelDimensionsException
	: Exception
{
	public InvalidParcelDimensionsException(decimal[] dimensions)
		: base()
	{
		this._message = $"Parcel Dimensions are invalid. ({string.Join('x', dimensions)})";

		this.Dimensions = dimensions;
	}

	private string _message;
	public override string Message => this._message;

	public decimal[] Dimensions { get; }
}