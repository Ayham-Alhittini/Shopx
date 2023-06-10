using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Extensions;
using Shopx.API.Interfaces;
using Stripe;
using System.ComponentModel.DataAnnotations;

namespace Shopx.API.Controllers
{
    public class ReviewController : BaseApiController
    {
        private IReviewRepository _reviewRepository;
        private UserManager<AppUser> _userManager;
        private IMapper _mapper;
        public ReviewController(IReviewRepository reviewRepository, IMapper mapper,
            UserManager<AppUser> userManager)
        {
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _mapper = mapper;
        }


        [Authorize(Roles = "Customer")]
        [HttpPost("post-product-review")]
        public async Task<ActionResult> PostProductReview(CreateProductReviewDto review)
        {
            ////check that the customer buy this product in order to review it

            if (!await _reviewRepository.CheckCustomerCouldReviewProduct(User.GetUserId(), review.ProductId))
                return BadRequest("You can review product when you buy it only");

            ///check if customer review this product before

            if (await _reviewRepository.GetProductReview(User.GetUserId(), review.ProductId) != null)
                return BadRequest("You review this product before, edit your review instead");


            ProductReview productReview = _mapper.Map<ProductReview>(review);
            productReview.CustomerId = User.GetUserId();


            _reviewRepository.PostProductReview(productReview);

            await _reviewRepository.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("post-product-review-vote/{reviewId}")]
        public async Task<ActionResult> PostProductReviewVote(int reviewId, [FromBody] int value)
        {
            ///check if review exist
            var review = await _reviewRepository.GetProductReviewByIdAsync(reviewId);

            if (review == null)
                return NotFound("Review not exist");
            
            ///check value value
            if (value != 1 && value != -1)
            {
                return BadRequest("Send (1) for like, and (-1) for dislike");
            }

            string response = "";
            ///check if vote before

            var previousReviewVote = await _reviewRepository.ProductReviewVote(User.GetUserId(), reviewId);

            if (previousReviewVote != null)
            {
                if (previousReviewVote.VoteValue == value)
                {
                    _reviewRepository.RemoveProductReviewVote(previousReviewVote);

                    response = value == 1 ? "Your like is deleted" : "Your dislike is deleted";
                }
                else
                {
                    previousReviewVote.VoteValue = value;

                    response = value == 1 ? "Vote changed from dislike to like" : "Vote changed from like to dislike";
                }
            }
            else
            {
                var vote = new ProductReviewVote
                {
                    UserId = User.GetUserId(),
                    ProductReviewId = reviewId,
                    VoteValue = value
                };

                _reviewRepository.AddProductReviewVote(vote);

                response = value == 1 ? "Like vote is added" : "Dislike vote is added";
            }

            await _reviewRepository.SaveChangesAsync();

            return Ok(new
            {
                Response = response,
            });
        }

        [Authorize]
        [HttpPost("post-shop-review-vote/{reviewId}")]
        public async Task<ActionResult> PostShopReviewVote(int reviewId, [FromBody] int value)
        {
            ///check review exist
            var review = await _reviewRepository.GetShopReviewByIdAsync(reviewId);

            if (review == null)
                return NotFound("Review not exist");

            ///check the value

            if (value != 1 && value != -1)
                return BadRequest("Send (1) for like, and (-1) for dislike");

            ///check if vote before

            var previousVote = await _reviewRepository.ShopReviewVote(User.GetUserId(), reviewId);

            string response = "";

            if (previousVote != null)
            {
                if (previousVote.VoteValue == value)
                {
                    //remove vote
                    _reviewRepository.RemoveShopReviewVote(previousVote);
                    
                    response = value == 1 ? "Your like is deleted" : "Your dislike is deleted";
                }
                else
                {
                    //change vote
                    previousVote.VoteValue = value;

                    response = value == 1 ? "Vote changed from dislike to like" : "Vote changed from like to dislike";
                }
            }
            else
            {
                var vote = new ShopReviewVote
                {
                    UserId = User.GetUserId(),
                    ShopReviewId = reviewId,
                    VoteValue = value
                };


                _reviewRepository.AddShopReviewVote(vote);

                response = value == 1 ? "Like vote is added" : "Dislike vote is added";

            }

            await _reviewRepository.SaveChangesAsync();

            return Ok(new
            {
                Response = response,
            });
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("post-shop-review")]
        public async Task<ActionResult> PostShopReview(CreateShopReviewDto reviewDto)
        {
            var seller = await _userManager.FindByNameAsync(reviewDto.SellerName);


            ///check shop exist
            if (seller == null)
                return NotFound("Shop not exist");


            ///check if customer could rate the shop
            if (!await _reviewRepository.CheckCustomerCouldReviewShop(User.GetUserId(), seller.Id))
                return BadRequest("Buy a product to review the shop");

            ShopReview shopReview = _mapper.Map<ShopReview>(reviewDto);
            shopReview.CustomerId = User.GetUserId();
            shopReview.SellerId = seller.Id;

            _reviewRepository.PostShopReview(shopReview);

            await _reviewRepository.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = "Customer")]
        [HttpPut("edit-product-review")]
        public async Task<ActionResult> EditProductReview(CreateProductReviewDto reviewDto)
        {
            var review = await _reviewRepository.GetProductReview(User.GetUserId(), reviewDto.ProductId);

            if (review == null)
                return NotFound("You not review this product");

            review.RatingValue = reviewDto.RatingValue;
            review.ReviewContent = reviewDto.ReviewContent;

            return Ok();
        }

        [Authorize(Roles = "Customer")]
        [HttpPut("edit-shop-review")]
        public async Task<ActionResult> EditShopReview(CreateShopReviewDto shopReviewDto)
        {
            var shop = await _userManager.FindByNameAsync(shopReviewDto.SellerName);

            if (shop == null)
                return NotFound("Shop not exist");

            var review = await _reviewRepository.GetShopReview(User.GetUserId(), shop.Id);
            if (review == null)
                return BadRequest("You not review this shop");

            review.RatingValue = shopReviewDto.RatingValue;
            review.ReviewContent = shopReviewDto.ReviewContent;

            return Ok();
        }

        [Authorize(Roles = "Customer")]
        [HttpDelete("delete-product-review/{productId}")]
        public async Task<ActionResult> DeleteReview(int productId)
        {
            var review = await _reviewRepository.GetProductReview(User.GetUserId(), productId);

            if (review == null)
                return NotFound();

            _reviewRepository.DeleteProductReview(review);

            await _reviewRepository.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Customer")]
        [HttpDelete("delete-shop-review/{shopname}")]
        public async Task<ActionResult> DeleteShopReview(string shopname)
        {
            var shop = await _userManager.FindByNameAsync(shopname);
            if (shop == null)
                return NotFound("Shop not exist");

            var review = await _reviewRepository.GetShopReview(User.GetUserId(), shop.Id);

            if (review == null)
                return BadRequest("You not review this shop");

            _reviewRepository.DeleteShopReview(review);

            await _reviewRepository.SaveChangesAsync();
            return Ok();
        }
    }
}
