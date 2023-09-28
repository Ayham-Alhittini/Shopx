using Shopx.API.Data;
using Shopx.API.Entities.Product_Specification;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopx.API.Entities
{
    [Table("Products")]
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }///product title
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public double Price { get; set; }
        public int DiscountRate { get; set; }
        public int Quantity { get; set; }
        public string SellerId { get; set; }
        public string SellerName { get; set; }
        public string ProductDescription { get; set; }
        public string State { get; set; } = States.active;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public AppUser Seller { get; set; }
        public List<Sales> BoughtProduct { get; set; }
        public List<ShoppingCart> Carts { get; set; }
        public List<ProductPhoto> ProductPhotos { get; set; } = new();
        public List<ProductReview> ProductReviews { get; set; }
        public List<BrowseHistory> BrowseHistories { get; set; }
        public List<ReportProduct> ReportProducts { get; set; }
        public int ProductViews { get; set; }

        ////product specification
        public int? LaptopId { get; set; }
        public LaptopAndComputer Laptop { get; set; }///laptop and computer

        public int? VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public int? PetId { get; set; }
        public Pet Pet { get; set; }

        public int? MobileId { get; set; }
        public Mobile Mobile { get; set; }

        public int? AccessoriesId { get; set; }
        public Accessories Accessories { get; set; }

        public int? MonitorProductId { get; set; }
        public MonitorProduct MonitorProduct { get; set; }
    }
}
