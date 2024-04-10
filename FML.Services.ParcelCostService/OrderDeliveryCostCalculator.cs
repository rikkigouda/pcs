using FML.Services.ParcelCostService.Pricing;
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
			this.ParcelProcessors = [
				new ParcelDimensionStrategy(),
				new ParcelWeightLimitStrategy(),
				new HeavyParcelStrategy(),
			];

			this.OrderPricing = [
				new ParcelBaseCostPricing(),
				new OverWeightParcelPricing(),
				new HeavyWeightParcelPricing(),
				new SpeedyShippingPricing(),
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
