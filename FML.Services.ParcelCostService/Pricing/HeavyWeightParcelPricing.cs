using FML.Services.ParcelCostService.Pricing.Model;

namespace FML.Services.ParcelCostService.Pricing
{
	public class HeavyWeightParcelPricing
		: BasePricing
	{
		public override void Calculate(PricingContext context)
		{
			decimal heavyWeight = 50;

			context.OrderProcessingContext.ParcelProcessingContexts
				.Where(item => item.IsHeavy == true)
				.ToList()
				.ForEach(
					(parcelProcessingContext) =>
					{
						context.PricingContextItems.Add(
							new ParcelPricingContextItem(
								parcelProcessingContext,
								50 /* Fixed 50$ figure for heavy parcels */
							+	(parcelProcessingContext.Parcel.Weight - heavyWeight)
							)
						);
					}
				);
		}
	}
}