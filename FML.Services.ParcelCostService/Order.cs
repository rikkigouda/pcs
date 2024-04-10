
public class Order
{
	public Order()
		: base()
	{
		this.Parcels = new List<Parcel>();
	}

	public IList<Parcel> Parcels { get; private set; }

	public bool SpeedyShipping { get; set; }
}