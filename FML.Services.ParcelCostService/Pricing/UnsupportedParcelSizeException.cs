using System.Runtime.Serialization;

namespace FML.Services.ParcelCostService.Pricing
{
	[Serializable]
	internal class UnsupportedParcelSizeException : Exception
	{
		private ParcelSize size;

		public UnsupportedParcelSizeException()
		{
		}

		public UnsupportedParcelSizeException(ParcelSize size)
		{
			this.size = size;
		}

		public UnsupportedParcelSizeException(string? message) : base(message)
		{
		}

		public UnsupportedParcelSizeException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected UnsupportedParcelSizeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}