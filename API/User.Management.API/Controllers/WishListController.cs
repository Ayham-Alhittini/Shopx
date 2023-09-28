using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopx.API.Data;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Extensions;
using Shopx.API.Helper;
using Shopx.API.Interfaces;

namespace Shopx.API.Controllers
{
    [Authorize(Roles = "Customer")]
    public class WishListController: BaseApiController
    {
        private readonly IWishListRepository _wishListRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        public WishListController(IWishListRepository wishListRepository, IMapper mapper,
            IProductRepository productRepository, ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _wishListRepository = wishListRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        [HttpPost("add-to-wishlist/{productId}")]
        public async Task<ActionResult> AddToWishList(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null || product.State != States.active)
                return NotFound("Product not exist");

            ///check if already on wish list 

            var _wishList = await _wishListRepository.GetWishListAsync(User.GetUserId(), productId);
            if (_wishList != null)
            {
                return BadRequest("Product already on wish list");
            }


            WishList wishList = new WishList
            {
                CustomerId = User.GetUserId(),
                CustomerUsername = User.GetUsername(),
                SellerId = product.SellerId,
                SellerName = product.SellerName,
                ProductId = product.Id,
                ProductName = product.ProductName,
                Price = product.Price
            };

            await _wishListRepository.AddToWishList(wishList);

            if (await _wishListRepository.SaveChangesAsync())
                return Ok();

            return BadRequest("Something went wrong");
        }


        [HttpDelete("delete-from-wishlist/{productId}")]
        public async Task<ActionResult> DeleteFromWishList(int productId)
        {
            var wish = await _wishListRepository.GetWishListAsync(User.GetUserId(), productId);
            if (wish == null)
                return NotFound();

            _wishListRepository.RemoveFromWishList(wish);

            await _wishListRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("my-wishlist")]
        public async Task<ActionResult> MyWishList([FromQuery]PaginationParams paginationParams)
        {
            var result = await _wishListRepository.GetWishLists(User.GetUserId(), paginationParams);

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            InitializationCustomer(ref result, await _customerRepository.GetCarts(User.GetUserId()));


            return Ok(result);
        }

        //private methods
        private void InitializationCustomer(ref PagedList<ProductCardDto> products, List<ShoppingCart> carts)
        {
            Dictionary<int, bool> existOnCart = new Dictionary<int, bool>();

            foreach (var cart in carts)
            {
                existOnCart[cart.ProductId] = true;
            }


            foreach (var product in products)
            {
                if (existOnCart.ContainsKey(product.Id))
                {
                    product.OnCart = true;
                }
                product.OnWishlist = true;
            }
        }
    }
}
