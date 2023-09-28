using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Entities.Product_Specification;
using Shopx.API.Interfaces;

namespace Shopx.API.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public void AddLaptop(LaptopAndComputer laptop)
        {
            _context.Laptops.Add(laptop);
        }

        public void AddVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ProductDto> GetProductDtoAsync(int productId, string accountType)
        {
            var product = _context.Products.Where(pro => pro.Id == productId);

            if (accountType != "Admin")
            {
                product = product.Where(pro => pro.State == States.active);
            }

            return await product
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }
    }
}
