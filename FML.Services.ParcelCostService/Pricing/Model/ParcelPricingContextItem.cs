using FML.Services.ParcelCostService.Processing.Model;

namespace FML.Services.ParcelCostService.Pricing.Model
{
    public class ParcelPricingContextItem
        : PricingContextItem
    {
        public ParcelPricingContextItem(
            ParcelProcessingContext parcelProcessingContext,
            decimal cost
        )
            : base(cost, PricingContextItemType.ItemisedCost)
        {
            ParcelProcessingContext = parcelProcessingContext;
        }

        public ParcelProcessingContext ParcelProcessingContext { get; private set; }
    }
}