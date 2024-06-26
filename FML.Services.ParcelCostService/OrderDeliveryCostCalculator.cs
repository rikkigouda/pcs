﻿using FML.Services.ParcelCostService.Pricing;
using FML.Services.ParcelCostService.Pricing.Model;
using FML.Services.ParcelCostService.Processing;
using FML.Services.ParcelCostService.Processing.Model;

namespace FML.Services.ParcelCostService
{
    public class OrderDeliveryCostCalculator
	{
		public OrderDeliveryCostCalculator()
			: base()
		{
			// TODO: Dependency Injection and builder patterns come to mind in order to turn the following two set of services into a centralised control mechanism in order to support orchestrating orders and rules engine scenarios to load from a datasource in the future.
			this.ParcelProcessors = [
				new ParcelDimensionStrategy(),
				new ParcelWeightLimitStrategy(),
				new HeavyParcelStrategy(),
			];

			this.OrderPricing = [
				new ParcelBaseCostPricing(),
				new OverWeightParcelPricing(),
				new HeavyWeightParcelPricing(),
				new DiscountPricing(),
				new SpeedyShippingPricing(), // Speedy shipping applies after discounts are taken into account
				new TotalPricing(),
			];
		}

		public IParcelProcessor[] ParcelProcessors { get; set; }
		public BasePricing[] OrderPricing { get; }

		public PricingContextItem[] BuildPricingModel(Order order)
		{
			// Process Delivery Contexts
			var processingContexts = order.Parcels
				.Select(
					parcel =>
					{
						ParcelProcessingContext context = new ParcelProcessingContext(parcel);

						this.ParcelProcessors.ToList().ForEach(
							(processor) =>
							{
								processor.Setup(context);
							}
						);

						return context;
					}
				)
				.ToArray();

			// Apply Pricing Model
			PricingContext pricingContext = new PricingContext(
				new OrderProcessingContext(
					order,
					processingContexts
				)
			);

			this.OrderPricing.ToList().ForEach(
				pricing => pricing.Calculate(pricingContext)
			);

			// Return Delivery Costs
			return pricingContext.PricingContextItems.ToArray();
		}
	}
}
