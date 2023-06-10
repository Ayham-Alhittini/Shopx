using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Entities.Product_Specification;

namespace Shopx.API.Interfaces
{
    public interface IProductRepository
    {
        void Add(Product product);
        void AddLaptop(LaptopAndComputer laptop);
        void AddVehicle(Vehicle vehicle);
        Task<ProductDto> GetProductDtoAsync(int productId, string accountType);
        Task<Product> GetProductByIdAsync(int productId);
        Task<bool> SaveAllAsync();
    }
}
