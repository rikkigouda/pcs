using FML.Services.ParcelCostService.Processing.Model;

namespace FML.Services.ParcelCostService.Processing
{
    public class ParcelWeightLimitStrategy
		: IParcelProcessor
	{
		void IParcelProcessor.Setup(ParcelProcessingContext context)
		{
			switch (context.Size)
			{
				case ParcelSize.Small:

					context.WeightLimit = 1;
					break;

				case ParcelSize.Medium:

					context.WeightLimit = 3;
					break;

				case ParcelSize.Large:

					context.WeightLimit = 6;
					break;

				case ParcelSize.XL:

					context.WeightLimit = 10;
					break;

				default:

					throw new UnsupportedParcelSizeException(context.Size);
			}
		}
	}
}