
public class Order
{
	public Order()
		: base()
	{
		this.Parcels = new List<Parcel>();
	}

	public List<Parcel> Parcels { get; set; }

	public bool SpeedyShipping { get; set; }
}