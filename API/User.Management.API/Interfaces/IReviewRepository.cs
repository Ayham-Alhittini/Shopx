using Shopx.API.DTOs;
using Shopx.API.Entities;

namespace Shopx.API.Interfaces
{
    public interface IReviewRepository
    {
        ////product review
        void PostProductReview(ProductReview review);
        void DeleteProductReview(ProductReview review);

        Task<List<ReviewDto>> GetProductReviews(int productId);
        Task<bool> CheckCustomerCouldReviewProduct(string customerId, int productId);
        Task<ReviewDto> GetProductReviewDto(string customerId, int productId);
        Task<ProductReview> GetProductReview(string customerId, int productId);
        Task<ProductReview> GetProductReviewByIdAsync(int reviewId);
        Task<ShopReview> GetShopReviewByIdAsync(int reviewId);
        Task<ProductReviewVote> ProductReviewVote(string customerId, int reviewId);
        Task<ShopReviewVote> ShopReviewVote(string customerId, int reviewId);
        void AddProductReviewVote(ProductReviewVote vote);
        void RemoveProductReviewVote(ProductReviewVote vote);

        void AddShopReviewVote(ShopReviewVote vote);
        void RemoveShopReviewVote(ShopReviewVote vote);

        ///shop review

        void PostShopReview(ShopReview shopReview);
        void DeleteShopReview(ShopReview shopReview);
        Task<bool> CheckCustomerCouldReviewShop(string customerId, string shopId);
        Task<List<ReviewDto>> GetShopReviews(string shopId);
        Task<ShopReview> GetShopReview(string customerId, string shopId);

        ///common
        Task<bool> SaveChangesAsync();
    }
}
