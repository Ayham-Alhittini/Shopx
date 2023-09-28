using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Helper;

namespace Shopx.API.Interfaces
{
    public interface IWishListRepository
    {
        Task AddToWishList(WishList wishList);
        void RemoveFromWishList(WishList wishList);
        Task<WishList> GetWishListAsync(string uesrId, int productId);
        Task<List<WishList>> GetWishListsAsync(string userId);
        Task<PagedList<ProductCardDto>> GetWishLists(string userId, PaginationParams paginationParams);
        Task<bool> SaveChangesAsync();
    }
}
