using Shopx.API.Entities;

namespace Shopx.API.DTOs
{
    public class SellerDto
    {
        ///common attribute 
        public string UserName { get; set; }
        public string KnownAs { get; set; }
        public bool isOnline { get; set; }
        public DateTime Created { get; set; }
        public PhotoDto BackgroundPhoto { get; set; }


        ///Seller attribute
        public string ShopCity { get; set; }
        public string ShopDescription { get; set; }
        public int ShopViews { get; set; }
        public ShopReviewDetails ShopReview { get; set; }

    }
}
