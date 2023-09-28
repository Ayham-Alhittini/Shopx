using Shopx.API.Entities;

namespace Shopx.API.DTOs
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }//product title
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ProductDescription { get; set; }
    }
}
