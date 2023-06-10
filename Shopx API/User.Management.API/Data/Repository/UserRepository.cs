using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Interfaces;

namespace Shopx.API.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<AppUser> GetUserByNameAsync(string name)
        {
            return await _context.Users.Include(p => p.BackgroundPhoto)
                .FirstOrDefaultAsync(x => x.UserName == name);
        }
        public async Task<SellerDto> GetSeller(string shopname)
        {
            var user = _context.Users.Where(u => u.UserName == shopname && u.AccountType == "Seller" && u.AccountState == States.active);

            var seller = await user.ProjectTo<SellerDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (seller == null)
            {
                return null;
            }

            return seller;
        }

        public async Task<CustomerDto> GetCustomer(string customerid)
        {
            return await _context.Users.Where(u => u.Id == customerid && u.AccountType == "Customer" && u.AccountState == States.active)
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }

        public async Task<ProductDto> GetProduct(int productid)
        {
            return await _context.Products.Where(pro => pro.Id == productid)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }

        public async Task<string> GetKnownAs(string userId)
        {
            return await _context.Users.Where(u => u.Id == userId)
                .Select(u => u.KnownAs).FirstOrDefaultAsync();
        }
    }
}
