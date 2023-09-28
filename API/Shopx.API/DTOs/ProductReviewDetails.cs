namespace Shopx.API.DTOs
{
    public class ProductReviewDetails
    {
        public double ProductRate { get; set; } = 0;
        public int NumberOfReviews { get; set; } = 0;


        public double FiveStarPercentage { get; set; } = 0;
        public int FiveStarCount { get; set; } = 0;


        public double FourStarPercentage { get; set; } = 0;
        public int FourStarCount { get; set; } = 0;

        public double ThreeStarPercentage { get; set; } = 0;
        public int ThreeStarCount { get; set; } = 0;

        public double TwoStarPercentage { get; set; } = 0;
        public int TwoStarCount { get; set; } = 0;

        public double OneStarPercentage { get; set; } = 0;
        public int OneStarCount { get; set; } = 0;

        public List<ReviewDto> ProductReviews { get; set; } = new List<ReviewDto>();
    }
}
