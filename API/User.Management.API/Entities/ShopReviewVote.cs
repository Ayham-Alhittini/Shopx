namespace Shopx.API.Entities
{
    public class ShopReviewVote
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int VoteValue { get; set; }
        /// <summary>
        /// if vote value  = +1 then like
        /// if note value  = -1 then dislike
        /// </summary>

        public int ShopReviewId { get; set; }  
        public ShopReview ShopReview { get; set; }
    }
}
