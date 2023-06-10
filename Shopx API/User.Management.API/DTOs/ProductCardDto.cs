using Shopx.API.Data;
using Shopx.API.DTOs.Product_Specification;

namespace Shopx.API.DTOs
{
    public class ProductCardDto
    {
        public int Id { get; set; }
        public string link { get; set; }
        public bool OnCart { get; set; }
        public bool OnWishlist { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public double Price { get; set; }
        public double PriceAfterDiscount { get; set; }
        public string SellerName { get; set; }
        public int DiscountRate { get; set; }
        public int Quantity { get; set; }
        public string State { get; set; }
        public int ReportCount { get; set; } = 0;
        public List<PhotoDto> ProductPhotos { get; set; } = new();
    }
}
