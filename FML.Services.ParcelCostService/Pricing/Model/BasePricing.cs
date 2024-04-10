namespace FML.Services.ParcelCostService.Pricing.Model
{
    public abstract class BasePricing
    {
        public abstract void Calculate(PricingContext context);
    }
}
