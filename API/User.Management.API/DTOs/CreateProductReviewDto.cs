using Shopx.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shopx.API.DTOs
{
    public class CreateProductReviewDto
    {
        public int ProductId { get; set; }
        [Required]
        [Range(1, 5)]
        public int RatingValue { get; set; }
        public string ReviewContent { get; set; } = "";
    }
}
