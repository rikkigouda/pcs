using FML.Services.ParcelCostService.Processing.Model;

namespace FML.Services.ParcelCostService.Processing
{
    public class HeavyParcelStrategy
		: IParcelProcessor
	{
		void IParcelProcessor.Setup(ParcelProcessingContext context)
		{
			if (context.Parcel.Weight <= 50)
			{
				return;
			}

			context.IsHeavy = true;
		}
	}
}