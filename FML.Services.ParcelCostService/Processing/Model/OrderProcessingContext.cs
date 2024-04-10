namespace FML.Services.ParcelCostService.Processing.Model
{
    public class OrderProcessingContext
    {
        public OrderProcessingContext(
            Order order,
            ParcelProcessingContext[] parcelProcessingContext
        )
        {
            Order = order;

            ParcelProcessingContexts = parcelProcessingContext;
        }

        public Order Order { get; private set; }

        public bool SpeedyDelivery { get; set; }

        public ParcelProcessingContext[] ParcelProcessingContexts { get; set; }
    }
}
