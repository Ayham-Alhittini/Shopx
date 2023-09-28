namespace Shopx.API.Entities
{
    public class ProductReviewVote
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int VoteValue { get; set; }
        /// <summary>
        /// if vote value  = +1 then like
        /// if note value  = -1 then dislike
        /// </summary>

        public int ProductReviewId { get; set; }
        public ProductReview ProductReview { get; set; }
    }
}
