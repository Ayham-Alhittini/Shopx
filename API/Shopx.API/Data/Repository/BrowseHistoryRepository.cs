using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using Microsoft.EntityFrameworkCore;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Helper.Filter_Params;
using Shopx.API.Helper;
using Shopx.API.Interfaces;
using Stripe;

namespace Shopx.API.Data.Repository
{
    public class BrowseHistoryRepository : IBrowseHistoryRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public BrowseHistoryRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public void Browse(BrowseHistory browseHistory)
        {
            _context.BrowseHistories.Add(browseHistory);
        }

        public void ClearBrowseHistory(List<BrowseHistory> browseHistories)
        {
            _context.BrowseHistories.RemoveRange(browseHistories);
        }

        public void DeleteBrowseHistory(BrowseHistory browseHistory)
        {
            _context.BrowseHistories.Remove(browseHistory);
        }

        public async Task<PagedList<ProductCardDto>> GetBrowsedProducts(string customerId, PaginationParams paginationParams)
        {
            var products =  _context.BrowseHistories
                .Where(b => b.CustomerId == customerId)
                .Select(b => b.Product)
            .AsQueryable();

            var result = await PagedList<ProductCardDto>
            .CreateAsync(products.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);

            return result;
        }

        public async Task<List<BrowseHistory>> GetBrowseHistories(string customerId)
        {
            return await _context.BrowseHistories
                .Where(b => b.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<BrowseHistory> GetBrowseHistory(string customerId, int productId)
        {
            return await _context.BrowseHistories.FindAsync(customerId, productId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
