using FML.Services.ParcelCostService.Processing.Model;

namespace FML.Services.ParcelCostService.Pricing.Model
{
    public class PricingContext
    {
        public PricingContext(
            OrderProcessingContext orderProcessingContext
        )
            : base()
        {
            OrderProcessingContext = orderProcessingContext;

            PricingContextItems = new List<PricingContextItem>();
        }

        public OrderProcessingContext OrderProcessingContext { get; private set; }
        public List<PricingContextItem> PricingContextItems { get; private set; }
    }
}