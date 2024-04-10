using FML.Services.ParcelCostService.Pricing.Model;

namespace FML.Services.ParcelCostService.Pricing
{
	public class OverWeightParcelPricing
		: BasePricing
	{
		public override void Calculate(PricingContext context)
		{
			List<PricingContextItem> pricingContextItems = new List<PricingContextItem>();

			context.OrderProcessingContext.ParcelProcessingContexts
				.Where(item => item.Parcel.Weight > item.WeightLimit)
				.ToList()
				.ForEach(
					(parcelProcessingContext) =>
					{
						var extraWeight = parcelProcessingContext.Parcel.Weight - parcelProcessingContext.WeightLimit;

						context.PricingContextItems.Add(
							new ParcelPricingContextItem(
								parcelProcessingContext,
								extraWeight * parcelProcessingContext.ExtraCostPerKg
							)
						);
					}
				);
		}
	}
}