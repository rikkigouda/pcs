using FML.Services.ParcelCostService.Pricing.Model;
using FML.Services.ParcelCostService.Processing.Model;

namespace FML.Services.ParcelCostService.Pricing
{
    public class DiscountPricing
		: BasePricing
	{
		public override void Calculate(PricingContext context)
		{
			List<PricingContextItem> discountedItems = new List<PricingContextItem>();
			List<ParcelPricingContextItem> parcelPricingContextsCopy = new List<ParcelPricingContextItem>(context.PricingContextItems.OfType<ParcelPricingContextItem>());

			var discountPacks = new (ParcelSize?, int) []
			{
				// Small parcel mania! Every 4th small parcel in an order is free!
				(ParcelSize.Small, 4),
				// Medium parcel mania! Every 3rd medium parcel in an order is free!
				(ParcelSize.Medium, 3),
				// Mixed parcel mania! Every 5th parcel in an order is free!
				(null, 5)
			};

			foreach (var pack in discountPacks)
			{
				IEnumerable<PricingContextItem> discountedPricingContextItems = this.CalculateDiscount(parcelPricingContextsCopy, pack.Item1, pack.Item2);

				discountedItems.AddRange(discountedPricingContextItems);

				if (discountedPricingContextItems.Count() > 0)
				{
					break;
				}
			}

			// Include the discounted items in the pricing model.
			context.PricingContextItems.AddRange(discountedItems);
		}

		private IEnumerable<PricingContextItem> CalculateDiscount(
			List<ParcelPricingContextItem> parcelPricingContextsCopy,
			ParcelSize? parcelSize,
			int batchSize
		)
		{
			int parcelsCount = parcelPricingContextsCopy
				.Where(item => item.PricingContextItemType == PricingContextItemType.ItemisedCost)
				.Where(item => !parcelSize.HasValue || item.ParcelProcessingContext.Size == parcelSize).Count();

			int batchCount = parcelsCount / batchSize;

			for (int i = 0; i < batchCount; i++)
			{
				var currentBatch = parcelPricingContextsCopy
					.Where(item => item.PricingContextItemType == PricingContextItemType.ItemisedCost)
					.Skip(i * batchSize)
					.Take(batchSize);

				// Within each discount, the cheapest parcel is the free one
				var cheapestItem = currentBatch.OrderBy(item => item.Cost).First();

				// REVIEW: The following commented line attempts to enforce the single use of each parcel across the discounts, but the requirement requires a bit of clarification around the definition of "used" in "Each parcel can only be *used* in a discount once"
				// parcelPricingContextsCopy.RemoveAll(item => currentBatch.Contains(item));

				parcelPricingContextsCopy.Remove(cheapestItem);

				var specialCostSum = parcelPricingContextsCopy.Where(item => item.ParcelProcessingContext == cheapestItem.ParcelProcessingContext && item.PricingContextItemType == PricingContextItemType.SpecialCost).Sum(item => item.Cost);

				yield return new ParcelPricingContextItem(
					cheapestItem.ParcelProcessingContext,
					(
					cheapestItem.Cost
				+	specialCostSum
				)
				*	-1 /* Discounts are added as individual negative items, hence multiply the cost by negative 1. */,
					PricingContextItemType.Discount
				);
			}
		}
	}
}