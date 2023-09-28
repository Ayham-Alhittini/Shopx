using System.ComponentModel.DataAnnotations;

namespace Shopx.API.DTOs
{
    public class CreateShopReviewDto
    {
        public string SellerName { get; set; }
        [Required]
        [Range(1, 5)]
        public int RatingValue { get; set; }
        public string ReviewContent { get; set; } = "";
    }
}
