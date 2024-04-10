using FML.Services.ParcelCostService.Processing;

namespace FML.Services.ParcelCostService.Pricing.Model
{
    public class PricingContextItem
    {
        public PricingContextItem(
            decimal cost,
            PricingContextItemType pricingContextItemType
        )
            : base()
        {
            Cost = cost;

            PricingContextItemType = pricingContextItemType;
        }

        public PricingContextItemType PricingContextItemType { get; private set; }

        public decimal Cost { get; private set; }
    }
}