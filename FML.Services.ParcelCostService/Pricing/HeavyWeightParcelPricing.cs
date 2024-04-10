using FML.Services.ParcelCostService.Pricing.Model;

namespace FML.Services.ParcelCostService.Pricing
{
	public class HeavyWeightParcelPricing
		: BasePricing
	{
		public override void Calculate(PricingContext context)
		{
			decimal heavyWeight = 50;

			context.PricingContextItems.OfType<ParcelPricingContextItem>()
				.Where(item => item.ParcelProcessingContext.IsHeavy == true)
				.ToList()
				.ForEach(
					(parcelPricingContextItem) =>
					{
						context.PricingContextItems.Remove(parcelPricingContextItem);

						context.PricingContextItems.Add(
							new ParcelPricingContextItem(
								parcelPricingContextItem.ParcelProcessingContext,
								50 /* Fixed 50$ figure for heavy parcels */
							+	(parcelPricingContextItem.ParcelProcessingContext.Parcel.Weight - heavyWeight)
							)
						);
					}
				);
		}
	}
}