namespace Shopx.API.Entities
{
    public class WishList
    {
        public string CustomerId { get; set; }
        public string CustomerUsername { get; set; }
        public AppUser Customer { get; set; }

        public string SellerId { get; set; }
        public string SellerName { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public Product Product { get; set; }
    }
}
