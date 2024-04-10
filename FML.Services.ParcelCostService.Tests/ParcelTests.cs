
namespace FML.Services.ParcelCostService.Tests
{
	[TestFixture]
	internal class ParcelTests
	{
		[Test]
		[TestCase(0, 10, 4.5)]
		[TestCase(4, 100, 0)]
		[TestCase(-1, 10, 4.5)]
		[TestCase(2, 10, -10)]
		public void EnsureInvalidParcelDimensionValuesThrowException(decimal width, decimal height, decimal length)
		{
			//Arrange

			//Act
			InvalidParcelDimensionsException invalidParcelDimensionsException = Assert.Catch<InvalidParcelDimensionsException>(
				() =>
				{
					var parcel = new Parcel(
						width: width,
						height: height,
						length: length,
						weight: 1
					);
				}
			);

			//Assert
			Assert.That(invalidParcelDimensionsException, Is.Not.Null);
			Assert.That(invalidParcelDimensionsException.Message, Is.EqualTo($"Parcel Dimensions are invalid. ({width}x{height}x{length})"));
		}
	}
}
