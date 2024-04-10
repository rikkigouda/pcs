using FML.Services.ParcelCostService.Pricing.Model;

namespace FML.Services.ParcelCostService.Pricing
{
    public class ParcelBaseCostPricing
		: BasePricing
	{
		public override void Calculate(PricingContext context)
		{
			List<PricingContextItem> pricingContextItems = new List<PricingContextItem>();

			context.OrderProcessingContext.ParcelProcessingContexts
				.ToList()
				.ForEach(
					(parcelProcessingContext) =>
					{
						decimal baseCost = 0;

						switch (parcelProcessingContext.Size)
						{
							case ParcelSize.Small:
								baseCost = 3;
								break;
							case ParcelSize.Medium:
								baseCost = 8;
								break;
							case ParcelSize.Large:
								baseCost = 15;
								break;
							case ParcelSize.XL:
								baseCost = 25;
								break;
							default:
								throw new UnsupportedParcelSizeException(parcelProcessingContext.Size);
						}

						context.PricingContextItems.Add(
							new ParcelPricingContextItem(
								parcelProcessingContext,
								baseCost
							)
						);
					}
				);
		}
	}
}