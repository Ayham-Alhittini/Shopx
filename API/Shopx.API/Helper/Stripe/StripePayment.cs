namespace Shopx.API.Helper.Stripe
{
    public record StripePayment(
        string CustomerId,
        string Description,
        string Currency,
        long Amount,
        string PaymentId);

}
