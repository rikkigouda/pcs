using FML.Services.ParcelCostService.Processing;

namespace FML.Services.ParcelCostService.Tests.Processing
{
    [TestFixture]
    internal class ParcelDimensionStrategyTests
    {
        /// <summary>
        ///	1) The initial implementation just needs to calculate cost based on a parcel’s size.For each size category there is a fixed delivery cost
        ///		○ Small parcel: all dimensions < 10cm.Cost $3
        ///		○ Medium parcel: all dimensions < 50cm.Cost $8
        ///		○ Large parcel: all dimensions < 100cm.Cost $15
        ///		○ XL parcel: any dimension >= 100cm.Cost $25
        /// </summary>
        /// <param name="width">Input Parcel Width</param>
        /// <param name="height">Input Parcel Height</param>
        /// <param name="length">Input Parcel Length</param>
        /// <param name="parcelSize">Expected Parcel Size given the dimensions</param>
        /// <param name="baseCost">Expected Base Cost given the dimensions</param>
        [Test]
        [TestCase(1, 9.99, 4.5, ParcelSize.Small, 3)]
        [TestCase(1, 10, 4.5, ParcelSize.Medium, 8)]
        [TestCase(49.99, 10, 4.5, ParcelSize.Medium, 8)]
        [TestCase(50, 10, 4.5, ParcelSize.Large, 15)]
        [TestCase(99.99, 10, 4.5, ParcelSize.Large, 15)]
        [TestCase(100, 10, 4.5, ParcelSize.XL, 25)]
        [TestCase(200, 10, 4.5, ParcelSize.XL, 25)]
        public void EnsureParcelDimensionValuesDetermineCorrectType(decimal width, decimal height, decimal length, ParcelSize parcelSize, decimal baseCost)
        {
            //Arrange
            IParcelProcessor parcelDimensionStrategy = new ParcelDimensionStrategy();
            var parcel = new Parcel(
                width: width,
                height: height,
                length: length,
                weight: 1
            );
            var context = new ParcelProcessingContext(parcel);

            //Act
            parcelDimensionStrategy.Setup(context);

            //Assert
            Assert.That(context.Size, Is.EqualTo(parcelSize));
            Assert.That(context.BaseCost, Is.EqualTo(baseCost));
        }
    }
}
