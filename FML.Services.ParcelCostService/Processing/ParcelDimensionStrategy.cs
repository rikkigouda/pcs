
namespace FML.Services.ParcelCostService.Processing
{
	public class ParcelDimensionStrategy
		: IParcelProcessor
	{
		void IParcelProcessor.Setup(ParcelProcessingContext context)
		{
			var maxDimension = new []
			{
				context.Parcel.Width,
				context.Parcel.Height,
				context.Parcel.Length
			}.Max();

			if (maxDimension < 10)
			{
				context.Size = ParcelSize.Small;
				context.BaseCost = 3;
			}
			else if (maxDimension < 50)
			{
				context.Size = ParcelSize.Medium;
				context.BaseCost = 8;
			}
			else if (maxDimension < 100)
			{
				context.Size = ParcelSize.Large;
				context.BaseCost = 15;
			}
			else
			{
				context.Size = ParcelSize.XL;
				context.BaseCost = 25;
			}
		}
	}
}