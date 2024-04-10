﻿using FML.Services.ParcelCostService.Pricing.Model;
using FML.Services.ParcelCostService.Processing.Model;
using FML.Services.ParcelCostService.Processing;
using System.Drawing;
using System.Diagnostics;
using System;

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
		public void EnsureCorrectOrderDeliveryCostCalculation()
		{
			// Arrange
			var order = new Order();

			order.Parcels.Add(new(width: 10, height: 10, length: 5, weight: 2));
			order.Parcels.Add(new(width: 2, height: 20, length: 3, weight: 25));
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
			order.Parcels.Add(new(width: 2, height: 20, length: 3, weight: 25));
			decimal expectedTotalCost = 32; // Including Speedy Shipment

			var orderCostCalculator = new OrderDeliveryCostCalculator();

			// Act
			var result = orderCostCalculator.BuildPricingModel(order);

			//Assert
			var total = result.Where(item => item.PricingContextItemType == PricingContextItemType.TotalCost);

			Assert.That(total.Count(), Is.EqualTo(1));
			Assert.That(total.Single().Cost, Is.EqualTo(expectedTotalCost));
		}
	}
}
