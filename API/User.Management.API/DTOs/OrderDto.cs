namespace Shopx.API.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string State { get; set; }
        public DateTime DateAdded { get; set; }
        public int NumberOfProducts { get; set; }
        public List<SalesDto> Sales { get; set; }
        public string CustomerKnownAs { get; set; }
        public string CustomerId { get; set; }
        public double Total { get; set; }
        public int AddressId { get; set; }
        public AddressDto Address { get; set; }
    }
}
