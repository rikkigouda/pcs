﻿using FML.Services.ParcelCostService.Pricing.Model;

namespace FML.Services.ParcelCostService.Pricing
{
    public class TotalPricing
		: BasePricing
	{
		public override void Calculate(PricingContext context)
		{
			context.PricingContextItems.Add(
				new PricingContextItem(
					context.PricingContextItems
						.Where(item => item.PricingContextItemType != PricingContextItemType.TotalCost)
						.Sum(item => item.Cost),
					PricingContextItemType.TotalCost
				)
			);
		}
	}
}