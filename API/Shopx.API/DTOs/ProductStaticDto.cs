namespace Shopx.API.DTOs
{
    public class ProductStaticDto
    {
        public string ProductName { get; set; }
        public int Id { get; set; }
        public int NumberOfCustomers { get; set; }
        public double Price { get; set; }
        public string State { get; set; }
        public int DiscountRate { get; set; }
        public int SolidQuantity { get; set; }//
        public int OnStock { get; set; }
        public int Views { get; set; }
        public double Total { get; set; }
    }
}
