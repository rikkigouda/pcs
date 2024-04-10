
namespace FML.Services.ParcelCostService.Processing
{
	public class ParcelProcessingContext
	{
		public ParcelProcessingContext(Parcel parcel)
			: base()
		{
			this.Parcel = parcel;

			this.ExtraCostPerKg = 2;
		}

		public Parcel Parcel { get; private set; }

		public ParcelSize Size { get; set; }

		public decimal BaseCost { get; set; }

		public decimal WeightLimit { get; set; }

		public decimal ExtraCostPerKg { get; private set; }
	}
}