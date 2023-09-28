using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Extensions;
using Shopx.API.Helper;
using Shopx.API.Interfaces;

namespace Shopx.API.Controllers
{
    [Authorize(Roles = "Customer")]
    public class BrowseHistoryController: BaseApiController
    {
        private readonly IBrowseHistoryRepository _browseHistoryRepository;
        private readonly IWishListRepository _wishListRepository;
        private readonly ICustomerRepository _customerRepository;
        public BrowseHistoryController(IBrowseHistoryRepository browseHistoryRepository, IWishListRepository wishListRepository, ICustomerRepository customerRepository)
        {
            _browseHistoryRepository = browseHistoryRepository;
            _wishListRepository = wishListRepository;
            _customerRepository = customerRepository;

        }

        [HttpGet("my-browse-history")]
        public async Task<ActionResult> MyBrowseHistory([FromQuery]PaginationParams paginationParams)
        {
            var result = await _browseHistoryRepository.GetBrowsedProducts(User.GetUserId(), paginationParams);

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            InitializationCustomer(ref result,
                    await _customerRepository.GetCarts(User.GetUserId()),
                    await _wishListRepository.GetWishListsAsync(User.GetUserId()));

            return Ok(result);
        }

        [HttpDelete("delete-from-browse-history/{productId}")]
        public async Task<ActionResult> DeleteFromBrowseHistory(int productId)
        {
            var browse = await _browseHistoryRepository.GetBrowseHistory(User.GetUserId(), productId);
            if (browse == null)
                return NotFound();

            _browseHistoryRepository.DeleteBrowseHistory(browse);


            return Ok();
        }

        [HttpDelete("clear-browse-history")]
        public async Task<ActionResult> ClearBrowseHistory()
        {
            var fullBrowse = await _browseHistoryRepository.GetBrowseHistories(User.GetUserId());

            if (fullBrowse.Count == 0)
                return BadRequest("You Don't have any browse history to clear");


            _browseHistoryRepository.ClearBrowseHistory(fullBrowse);

            await _browseHistoryRepository.SaveChangesAsync();

            return Ok();
        }

        ///private methods
        private void InitializationCustomer(ref PagedList<ProductCardDto> products, List<ShoppingCart> carts, List<WishList> wishes)
        {
            Dictionary<int, bool> existOnCart = new Dictionary<int, bool>();
            Dictionary<int, bool> existOnWishes = new Dictionary<int, bool>();

            foreach (var cart in carts)
            {
                existOnCart[cart.ProductId] = true;
            }

            foreach (var wish in wishes)
            {
                existOnWishes[wish.ProductId] = true;
            }


            foreach (var product in products)
            {
                if (existOnCart.ContainsKey(product.Id))
                {
                    product.OnCart = true;
                }
                if (existOnWishes.ContainsKey(product.Id))
                {
                    product.OnWishlist = true;
                }
            }
        }
    }
}
