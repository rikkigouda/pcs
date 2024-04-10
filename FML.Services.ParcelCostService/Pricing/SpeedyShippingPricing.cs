using FML.Services.ParcelCostService.Pricing.Model;

namespace FML.Services.ParcelCostService.Pricing
{
    public class SpeedyShippingPricing
		: BasePricing
	{
		public override void Calculate(PricingContext context)
		{
			if (context.OrderProcessingContext.Order.SpeedyShipping == false)
			{
				return;
			}

			context.PricingContextItems.Add(
				new PricingContextItem(
					context.PricingContextItems
						.Where(item => item.PricingContextItemType != PricingContextItemType.TotalCost)
						.Sum(item => item.Cost),
					PricingContextItemType.SpecialCost
				)
			);
		}
	}
}