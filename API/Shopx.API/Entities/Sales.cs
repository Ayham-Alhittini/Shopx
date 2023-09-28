using System.ComponentModel.DataAnnotations.Schema;

namespace Shopx.API.Entities
{
    public class Sales
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerUsername { get; set; }
        public AppUser Customer { get; set; }

        public string SellerId { get; set; }
        public string SellerName { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public double Total { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
