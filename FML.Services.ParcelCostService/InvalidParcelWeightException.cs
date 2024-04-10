using System.Runtime.Serialization;

namespace FML.Services.ParcelCostService
{
	[Serializable]
	internal class InvalidParcelWeightException : Exception
	{
		private decimal weight;

		public InvalidParcelWeightException(decimal weight)
		{
			this.weight = weight;
		}
	}
}