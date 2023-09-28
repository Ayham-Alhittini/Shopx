namespace Shopx.API.Helper.Stripe
{
    public record AddStripePayment(
        string CustomerId,
        string Description,
        string Currency,
        long Amount);
}
