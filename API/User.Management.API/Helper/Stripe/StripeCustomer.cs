namespace Shopx.API.Helper.Stripe
{
    public record StripeCustomer(
        string Name,
        string Email,
        string CustomerId);
}
