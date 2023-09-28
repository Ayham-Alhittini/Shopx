using Shopx.API.DTOs.Product_Specification;

namespace Shopx.API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string link { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public double Price { get; set; }
        public double PriceAfterDiscount { get; set; }
        public bool OnWishlist { get; set; }
        public bool OnCart { get; set; }
        public bool Reported { get; set; } 
        public int DiscountRate { get; set; }
        public int Quantity { get; set; }
        public string SellerId { get; set; }
        public string SellerName { get; set; }
        public string ProductDescription { get; set; }
        public string State { get; set; }
        public DateTime Created { get; set; }
        public int ReportCount { get; set; } = 0;
        public List<PhotoDto> ProductPhotos { get; set; } = new();
        public ProductReviewDetails ProductReview { get; set; }
        public string Specification { get; set; }


        ////product specification
        public LaptopDto Laptop { get; set; }
        public VehicleDto Vehicle { get; set; }
        public PetDto Pet { get; set; }
        public MobileDto Mobile { get; set; }
        public AccessoriesDto Accessories { get; set; }
        public MonitorProductDto MonitorProduct { get; set; }
    }
}
