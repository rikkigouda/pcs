using FML.Services.ParcelCostService.Pricing.Model;

namespace FML.Services.ParcelCostService.Pricing
{
	public class OverWeightParcelPricing
		: BasePricing
	{
		public override void Calculate(PricingContext context)
		{
			context.PricingContextItems.OfType<ParcelPricingContextItem>()
				.Where(item => item.ParcelProcessingContext.Parcel.Weight > item.ParcelProcessingContext.WeightLimit)
				.Where(item => item.ParcelProcessingContext.IsHeavy == false)
				.ToList()
				.ForEach(
					(parcelPricingContextItem) =>
					{
						var extraWeight = parcelPricingContextItem.ParcelProcessingContext.Parcel.Weight - parcelPricingContextItem.ParcelProcessingContext.WeightLimit;

						context.PricingContextItems.Add(
							new ParcelPricingContextItem(
								parcelPricingContextItem.ParcelProcessingContext,
								extraWeight * parcelPricingContextItem.ParcelProcessingContext.ExtraCostPerKg,
								PricingContextItemType.SpecialCost
							)
						);
					}
				);
		}
	}
}