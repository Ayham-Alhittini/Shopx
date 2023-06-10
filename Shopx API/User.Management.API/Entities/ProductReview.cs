using System.ComponentModel.DataAnnotations;

namespace Shopx.API.Entities
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int RatingValue { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
        public string ReviewContent { get; set; } = "";

        public string CustomerId { get; set; }
        public AppUser Customer { get; set; }

        public List<ProductReviewVote> ProductReviewVotes { get; set; }
    }
}