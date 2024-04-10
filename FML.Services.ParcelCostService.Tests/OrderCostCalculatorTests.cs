using FML.Services.ParcelCostService.Pricing.Model;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace FML.Services.ParcelCostService.Tests
{
	[TestFixture]
	internal class OrderCostCalculatorTests
	{
		/// <summary>
		/// 1) The initial implementation just needs to calculate cost based on a parcel’s size. For each
		///		size category there is a fixed delivery cost
		///		○ Small parcel: all dimensions < 10cm.Cost $3
		///		○ Medium parcel: all dimensions < 50cm.Cost $8
		///		○ Large parcel: all dimensions < 100cm.Cost $15
		///		○ XL parcel: any dimension >= 100cm.Cost $25
		///		
		/// > Output should be a collection of items with their individual cost and type, as well as the total cost
		/// </summary>
		[Test]
		public void EnsureCorrectOrderDeliveryCostBaseCalculation()
		{
			// Arrange
			var order = new Order();

			order.Parcels.Add(new(width: 10, height: 10, length: 5, weight: 2));
			order.Parcels.Add(new(width: 2, height: 20, length: 3, weight: 3));
			decimal expectedTotalCost = 16;

			var orderCostCalculator = new OrderDeliveryCostCalculator();

			// Act
			var result = orderCostCalculator.BuildPricingModel(order);

			//Assert
			var parcelCosts = result.Where(item => item.PricingContextItemType == PricingContextItemType.ItemisedCost);
			var total = result.Where(item => item.PricingContextItemType == PricingContextItemType.TotalCost);

			Assert.That(parcelCosts.Count(), Is.EqualTo(order.Parcels.Count));
			Assert.That(total.Count(), Is.EqualTo(1));

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

			Assert.That(total.Single().Cost, Is.EqualTo(expectedTotalCost));
		}

		/// <summary>
		/// 2) Thanks to logistics improvements we can deliver parcels faster. This means we can charge more money.Speedy shipping can be selected by the user to take advantage of our improvements.
		///		○ Speedy shipping doubles the cost of the entire order
		///		○ Speedy shipping should be listed as a separate item in the output, with its associated cost
		///		○ Speedy shipping should not impact the price of individual parcels, i.e.their individual cost should remain the same as it was before
		/// </summary>
		[Test]
		public void EnsureCorrectOrderDeliveryCostCalculationWithSpeedyShipping()
		{
			// Arrange
			var order = new Order()
			{
				SpeedyShipping = true
			};

			order.Parcels.Add(new(width: 10, height: 10, length: 5, weight: 2));
			order.Parcels.Add(new(width: 2, height: 20, length: 3, weight: 3));
			decimal expectedTotalCost = 32; // Including Speedy Shipment

			var orderCostCalculator = new OrderDeliveryCostCalculator();

			// Act
			var result = orderCostCalculator.BuildPricingModel(order);

			//Assert
			var total = result.Where(item => item.PricingContextItemType == PricingContextItemType.TotalCost);

			Assert.That(total.Count(), Is.EqualTo(1));
			Assert.That(total.Single().Cost, Is.EqualTo(expectedTotalCost));
		}

		/// <summary>
		/// 3) There have been complaints from delivery drivers that people are taking advantage of our dimension only shipping costs.A new weight limit has been added for each parcel type, over which a charge per kg applies +$2/kg over weight limit for parcel size:
		///		● Small parcel: 1kg
		///		● Medium parcel: 3kg
		///		● Large parcel: 6kg
		///		● XL parcel: 10kg
		/// </summary>
		[Test]
		public void EnsureCorrectOrderDeliveryCostCalculationForOvereightParcels()
		{
			// Arrange
			var order = new Order()
			{
				SpeedyShipping = false
			};

			order.Parcels.Add(new(width: 10, height: 10, length: 5, weight: 2));
			order.Parcels.Add(new(width: 2, height: 20, length: 3, weight: 4));
			decimal expectedTotalCost =
				16  /* Without SpeedyShipment */
			+ 2 /* With 2kg extra for the second Medium sized parcel. */
			;

			var orderCostCalculator = new OrderDeliveryCostCalculator();

			// Act
			var result = orderCostCalculator.BuildPricingModel(order);

			//Assert
			var total = result.Where(item => item.PricingContextItemType == PricingContextItemType.TotalCost);

			Assert.That(total.Count(), Is.EqualTo(1));
			Assert.That(total.Single().Cost, Is.EqualTo(expectedTotalCost));
		}

		/// <summary>
		/// 4) Some of the extra weight charges for certain goods were excessive. A new parcel type has been added to try and address overweight parcels
		///
		///		Heavy parcel, $50 up to 50kg +$1/kg over 50kg
		/// </summary>
		[Test]
		public void EnsureCorrectOrderDeliveryCostCalculationForHeavyParcels()
		{
			// Arrange
			var order = new Order()
			{
				SpeedyShipping = false
			};

			order.Parcels.Add(new(width: 10, height: 10, length: 5, weight: 2));
			order.Parcels.Add(new(width: 2, height: 20, length: 3, weight: 51));
			decimal expectedTotalCost =
				8  /* Without SpeedyShipment */
			+ 51  /* With 50$ for Heavy parcel and 1$ for 1kg extra of the same parcel. */
			;

			var orderCostCalculator = new OrderDeliveryCostCalculator();

			// Act
			var result = orderCostCalculator.BuildPricingModel(order);

			//Assert
			var total = result.Where(item => item.PricingContextItemType == PricingContextItemType.TotalCost);

			Assert.That(total.Count(), Is.EqualTo(1));
			Assert.That(total.Single().Cost, Is.EqualTo(expectedTotalCost));
		}

		/// <summary>
		/// 5) In order to award those who send multiple parcels, special discounts have been introduced.
		///		● Small parcel mania! Every 4th small parcel in an order is free!
		///		● Medium parcel mania! Every 3rd medium parcel in an order is free!
		///		● Mixed parcel mania! Every 5th parcel in an order is free!
		///		● Each parcel can only be used in a discount once
		///		● Within each discount, the cheapest parcel is the free one
		///		● The combination of discounts which saves the most money should be selected every time
		///		
		///		Example:
		///			6x medium parcels. 3x $8, 3 x $10. 1st discount should include all 3 $8 parcels and save $8. 2nd discount should include all 3 $10 parcels and save $10.
		///			● Just like speedy shipping, discounts should be listed as a separate item in the output, with associated saving, e.g. “-$2”
		///			● Discounts should not impact the price of individual parcels, i.e.their individual cost should remain the same as it was before
		///			● Speedy shipping applies after discounts are taken into account
		/// </summary>
		[Test]
		public void EnsureCorrectOrderDeliveryCostCalculationForDiscountedParcels()
		{
			// Arrange
			var order = new Order()
			{
				SpeedyShipping = false
			};

			order.Parcels.Add(new(width: 10, height: 10, length: 5, weight: 2));
			order.Parcels.Add(new(width: 10, height: 10, length: 5, weight: 2));
			order.Parcels.Add(new(width: 10, height: 10, length: 5, weight: 2));
			order.Parcels.Add(new(width: 10, height: 10, length: 5, weight: 4));
			order.Parcels.Add(new(width: 10, height: 10, length: 5, weight: 4));
			order.Parcels.Add(new(width: 10, height: 10, length: 5, weight: 4));
			decimal expectedTotalCost =
				(3 * 8)  /* 3 Medium packages each 8$ */
			+	(3 * 10)  /* 6 Medium packages each 10$ (CLARIFICATION REQUIRED: How did the 2nd set of Medium packages become 10$? I assumed it happened because of extra weight charges.) */
			-	8  /* 1st 3rd item discount. */
			-	10  /* 2nd 3rd item discount. */
			;

			var orderCostCalculator = new OrderDeliveryCostCalculator();

			// Act
			var result = orderCostCalculator.BuildPricingModel(order);

			//Assert
			var discountedItems = result.Where(item => item.PricingContextItemType == PricingContextItemType.Discount);
			var total = result.Where(item => item.PricingContextItemType == PricingContextItemType.TotalCost);

			Assert.That(discountedItems.Count(), Is.EqualTo(2));
			Assert.That(total.Single().Cost, Is.EqualTo(expectedTotalCost));
		}
	}
}
