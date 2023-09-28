using Shopx.API.Entities;

namespace Shopx.API.DTOs
{
    /// <summary>
    /// for view the seller recived payment page
    /// </summary>
    public class PaymentDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerUsername { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
    }
}
