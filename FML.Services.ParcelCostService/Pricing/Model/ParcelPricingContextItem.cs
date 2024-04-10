using FML.Services.ParcelCostService.Processing.Model;

namespace FML.Services.ParcelCostService.Pricing.Model
{
    public class ParcelPricingContextItem
        : PricingContextItem
	{
		public ParcelPricingContextItem(
			ParcelProcessingContext parcelProcessingContext,
			decimal cost,
			PricingContextItemType pricingContextItemType
		)
			: base(cost, pricingContextItemType)
		{
			ParcelProcessingContext = parcelProcessingContext;
		}

		public ParcelPricingContextItem(
			ParcelProcessingContext parcelProcessingContext,
			decimal cost
		)
			: this(parcelProcessingContext, cost, PricingContextItemType.ItemisedCost)
		{ }

		public ParcelProcessingContext ParcelProcessingContext { get; private set; }
    }
}