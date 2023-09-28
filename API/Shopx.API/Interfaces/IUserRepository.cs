using Shopx.API.DTOs;
using Shopx.API.Entities;

namespace Shopx.API.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByIdAsync(string id);
        Task<bool> SaveAllAsync();
        Task<AppUser> GetUserByNameAsync(string name);
        Task<SellerDto> GetSeller(string shopname);
        Task<CustomerDto> GetCustomer(string customerid);
        Task<ProductDto> GetProduct(int productid);
        Task<string> GetKnownAs(string userId);
    }
}
