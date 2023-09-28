using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Helper;

namespace Shopx.API.Interfaces
{
    public interface ICustomerRepository
    {
        void buy(Sales sale);
        Task<PagedList<Order>> myOrders(string customerId, PaginationParams paginationParams);
        Task<OrderDto> GetOrderDetails(string customerId, int orderId);
        void AddToCart(ShoppingCart cart);
        Task<List<ShoppingCart>> GetCarts(string customerId);
        Task<ShoppingCart> GetCart(string customerId, int productId);
        void AddAddress(Address address);
        void AddOrder(Order order);
        void RemoveFromCart(ShoppingCart cart);
        void ClearCart(string customerId);
        Task<bool> SaveAllAsync();

        Task<bool> IsOnCart(string customerId, int productId);
    }
}
