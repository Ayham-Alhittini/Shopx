using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Helper;

namespace Shopx.API.Interfaces
{
    public interface ISellerRepository
    {
        Task<Product> GetProductAsync(int id, string sellerId);
        Task<PagedList<ProductCardDto>> GetProductsAsync(string sellerId, PaginationParams paginationParams);
        Task<IEnumerable<MessageDto>> GetProductMessages(int productId, string username);
        Task UpdateChangesToShopping (Product product);
        Task<PagedList<PaymentDto>> GetPayments(string sellerId, PaginationParams paginationParams);
        Task<int> GetShopViews(string shopname);
    }
}
