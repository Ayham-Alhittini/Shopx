using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Helper;
using Shopx.API.Helper.Filter_Params;
using Shopx.API.Interfaces;
using Stripe;

namespace Shopx.API.Data.Repository
{
    public class WishListRepository : IWishListRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IMessagesRepository _messagesRepository;
        public WishListRepository(DataContext context, IMapper mapper,
            IMessagesRepository messagesRepository)
        {
            _context = context;
            _mapper = mapper;
            _messagesRepository = messagesRepository;

        }
        public async Task AddToWishList(WishList wishList)
        {
            _context.Wishlists.Add(wishList);

            var message = new Message
            {
                Content = "I Add This Product To My Favorite",
                SenderUsername = wishList.CustomerUsername,
                SenderId = wishList.CustomerId,
                RecipenetUsername = wishList.SellerName,
                RecipenetId = wishList.SellerId,
                ProductId = wishList.ProductId,
            };

            await _messagesRepository.AddMessage(message);
        }

        public async Task<WishList> GetWishListAsync(string uesrId, int productId)
        {
            return await _context.Wishlists.FindAsync(uesrId, productId);
        }

        public async Task<PagedList<ProductCardDto>> GetWishLists(string userId, PaginationParams paginationParams)
        {
            var wishProducts = _context.Wishlists
                .Where(wish => wish.CustomerId == userId && wish.Product.State == States.active).Select(wish => wish.Product).AsQueryable();
            
            var result = await PagedList<ProductCardDto>
            .CreateAsync(wishProducts.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);

            return result;
        }

        public async Task<List<WishList>> GetWishListsAsync(string userId)
        {
            return await _context.Wishlists
                .Where(w => w.CustomerId == userId)
                .ToListAsync();
        }

        public void RemoveFromWishList(WishList wishList)
        {
            _context.Wishlists.Remove(wishList);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
