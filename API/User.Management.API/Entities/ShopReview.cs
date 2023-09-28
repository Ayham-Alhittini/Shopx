using System.ComponentModel.DataAnnotations;

namespace Shopx.API.Entities
{
    public class ShopReview
    {
        public int Id { get; set; } 
        public string SellerId { get; set; }
        public AppUser Seller { get; set; }

        public int RatingValue { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
        public string ReviewContent { get; set; } = "";

        public string CustomerId { get; set; }
        public AppUser Customer { get; set; }

        public List<ShopReviewVote> ShopReviewVotes { get; set; }
    }
}
