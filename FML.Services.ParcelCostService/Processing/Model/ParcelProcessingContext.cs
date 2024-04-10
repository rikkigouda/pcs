namespace FML.Services.ParcelCostService.Processing.Model
{
    public class ParcelProcessingContext
    {
        public ParcelProcessingContext(Parcel parcel)
            : base()
        {
            Parcel = parcel;

            ExtraCostPerKg = 2;
        }

        public Parcel Parcel { get; private set; }

        public ParcelSize Size { get; set; }

        public decimal WeightLimit { get; set; }

        public decimal ExtraCostPerKg { get; private set; }
    }
}