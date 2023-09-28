namespace Shopx.API.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string State { get; set; } = "Pending";
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public List<Sales> Sales { get; set; }
        public string CustomerId { get; set; }
        public string CustomerKnownAs { get; set; }
        public AppUser Customer { get; set; }
        public double Total { get; set; }
        public int AddressId { get; set; }
        public int NumberOfProducts { get; set; }
        public Address Address { get; set; }
        public string DeliveryOption { get; set; }
        public double DeliveryPrice { get; set; }
    }
}
