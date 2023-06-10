using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Interfaces;

namespace Shopx.API.Data.Repository
{
    public class ReviewRepository: IReviewRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ReviewRepository(DataContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }


        ///Product Repository
        public async Task<bool> CheckCustomerCouldReviewProduct(string customerId, int productId)
        {
            return await _context.Sales
                .Where(sales => sales.CustomerId == customerId && sales.ProductId == productId)
                .FirstOrDefaultAsync() != null;
        }

        public void DeleteProductReview(ProductReview review)
        {
            _context.ProductReviews.Remove(review);
        }

        public async Task<ProductReview> GetProductReview(string customerId, int productId)
        {
            return await _context.ProductReviews
                .Where(review => review.CustomerId == customerId && review.ProductId == productId)
                .FirstOrDefaultAsync();
        }

        public async Task<ReviewDto> GetProductReviewDto(string customerId, int productId)
        {
            return await _context.ProductReviews
                .Where(review => review.CustomerId == customerId && review.ProductId == productId)
                .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ReviewDto>> GetProductReviews(int productId)
        {
            return await _context.ProductReviews
                .Where(review => review.ProductId == productId)
                .Take(100)
                .OrderByDescending(review => review.ProductReviewVotes.Sum(v => v.VoteValue))
                .ThenByDescending(review => review.ReviewDate)
                .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public void PostProductReview(ProductReview review)
        {
            _context.ProductReviews.Add(review);
        }

        ///Shop Repository

        public void PostShopReview(ShopReview shopReview)
        {
            _context.ShopReviews.Add(shopReview);
        }

        public void DeleteShopReview(ShopReview shopReview)
        {
            _context.ShopReviews.Remove(shopReview);
        }

        public async Task<ShopReview> GetShopReview(string customerId, string shopId)
        {
            return await _context.ShopReviews.FindAsync(shopId, customerId);
        }

        public async Task<List<ReviewDto>> GetShopReviews(string shopId)
        {
            return await _context.ShopReviews
                .Where(r => r.SellerId == shopId)
                .Take(100)
                .OrderByDescending(review => review.ShopReviewVotes.Sum(v => v.VoteValue))
                .ThenByDescending(review => review.ReviewDate)
                .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> CheckCustomerCouldReviewShop(string customerId, string shopId)
        {
            return await _context.Sales
                .Where(sales => sales.CustomerId == customerId && sales.SellerId == shopId)
                .AnyAsync();
        }

        /// Common Repository
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ProductReviewVote> ProductReviewVote(string customerId, int reviewId)
        {
            return await _context.ProductReviewVotes.FindAsync(customerId, reviewId);
        }

        public void AddProductReviewVote(ProductReviewVote vote)
        {
            _context.ProductReviewVotes.Add(vote);
        }

        public void RemoveProductReviewVote(ProductReviewVote vote)
        {
            _context.ProductReviewVotes.Remove(vote);
        }

        public async Task<ProductReview> GetProductReviewByIdAsync(int reviewId)
        {
            return await _context.ProductReviews.FindAsync(reviewId);
        }

        public void AddShopReviewVote(ShopReviewVote vote)
        {
            _context.ShopReviewVotes.Add(vote);
        }

        public void RemoveShopReviewVote(ShopReviewVote vote)
        {
            _context.ShopReviewVotes.Remove(vote);
        }

        public async Task<ShopReview> GetShopReviewByIdAsync(int reviewId)
        {
            return await _context.ShopReviews.FindAsync(reviewId);
        }

        public async Task<ShopReviewVote> ShopReviewVote(string customerId, int reviewId)
        {
            return await _context.ShopReviewVotes.FindAsync(customerId, reviewId);
        }
    }
}
