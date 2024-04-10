
using FML.Services.ParcelCostService.Processing.Model;

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
			}
			else if (maxDimension < 50)
			{
				context.Size = ParcelSize.Medium;
			}
			else if (maxDimension < 100)
			{
				context.Size = ParcelSize.Large;
			}
			else
			{
				context.Size = ParcelSize.XL;
			}
		}
	}
}