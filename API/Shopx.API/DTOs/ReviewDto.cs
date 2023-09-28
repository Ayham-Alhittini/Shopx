using Shopx.API.Entities;

namespace Shopx.API.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int RatingValue { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ReviewContent { get; set; } = "";
        public int Initial { get; set; } = 0;
        public int VoteRating { get; set; } = 0;
        public CustomerDto Customer { get; set; }
    }
}
