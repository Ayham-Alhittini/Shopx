using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Helper;

namespace Shopx.API.Interfaces
{
    public interface IBrowseHistoryRepository
    {
        void Browse(BrowseHistory browseHistory);
        void DeleteBrowseHistory(BrowseHistory browseHistory);
        void ClearBrowseHistory(List<BrowseHistory> browseHistories);
        Task<BrowseHistory> GetBrowseHistory(string customerId, int productId);
        Task<List<BrowseHistory>> GetBrowseHistories(string customerId);
        Task<PagedList<ProductCardDto>> GetBrowsedProducts(string customerId, PaginationParams paginationParams);
        Task<bool> SaveChangesAsync();
    }
}
