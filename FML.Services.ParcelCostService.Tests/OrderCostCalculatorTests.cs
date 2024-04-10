using FML.Services.ParcelCostService.Pricing.Model;
using FML.Services.ParcelCostService.Processing.Model;
using FML.Services.ParcelCostService.Processing;

namespace FML.Services.ParcelCostService.Tests
{
	[TestFixture]
	internal class OrderCostCalculatorTests
	{
		[Test]
		public void EnsureCorrectOrderDeliveryCostCalculation()
		{
			// Arrange
			var order = new Order();

			order.Parcels.Add(new(width: 10, height: 10, length: 5, weight: 2));
			order.Parcels.Add(new(width: 2, height: 20, length: 3, weight: 25));

			var orderCostCalculator = new OrderDeliveryCostCalculator();

			// Act
			var result = orderCostCalculator.BuildPricingModel(order);

			//Assert
			var parcelCosts = result.Where(item => item.PricingContextItemType == PricingContextItemType.ItemisedCost);
			var total = result.Where(item => item.PricingContextItemType == PricingContextItemType.TotalCost);

			Assert.That(parcelCosts.Count(), Is.EqualTo(order.Parcels.Count));
			Assert.That(total.Count(), Is.EqualTo(1));


			IParcelProcessor parcelDimensionStrategy = new ParcelDimensionStrategy();
			result.OfType<ParcelPricingContextItem>().ToList().ForEach(item =>
			{
				decimal expectedBaseCost = 0;

				switch (item.ParcelProcessingContext.Size)
				{
					case ParcelSize.Small:

						expectedBaseCost = 3;
						break;
					case ParcelSize.Medium:

						expectedBaseCost = 8;
						break;
					case ParcelSize.Large:

						expectedBaseCost = 15;
						break;
					case ParcelSize.XL:

						expectedBaseCost = 25;
						break;
				}

				Assert.That(expectedBaseCost, Is.EqualTo(item.Cost));
			});
		}
	}
}
