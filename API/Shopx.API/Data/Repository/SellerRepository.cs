using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Helper;
using Shopx.API.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Shopx.API.Data.Repository
{
    public class SellerRepository : ISellerRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
        public SellerRepository(DataContext context, IMapper mapper, INotificationRepository notificationRepository)
        {
            _context = context;
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }

        public async Task UpdateChangesToShopping(Product product)
        {
            var carts = await _context.Carts
                .Where(cart => cart.ProductId == product.Id).ToListAsync();


            var validCarts = new List<ShoppingCart>();
            var invalidCarts = new List<ShoppingCart>();
            ////invalid because newQuantity is less that required quantity



            foreach(var  cart in carts)
            {
                if (product.Quantity >= cart.Quantity)
                {
                    cart.ProductName = product.ProductName;
                    cart.Price = product.Price - product.Price * product.DiscountRate / 100.0;
                    cart.Total = cart.Price * cart.Quantity;

                    validCarts.Add(cart);
                }
                else
                {
                    invalidCarts.Add(cart);
                }
            }

            foreach(var cart in validCarts)
            {
                Notification notification = new Notification
                {
                    Title = "Product has been changed",
                    Description = $"The seller ({cart.SellerName}) has edited the product that is in your cart, check if you still interested," +
                    $"product id : {cart.ProductId}",
                    UserId = cart.CustomerId

                };

                _notificationRepository.SendNotification(notification);

                await _notificationRepository.SaveChanges();
            }


            foreach (var cart in invalidCarts)
            {
                Notification notification = new Notification
                {
                    Title = "Product deleted from cart",
                    Description = $"Sorry for telling you that a product  ({cart.ProductName}) , " +
                    $"with id: {cart.ProductId}, is no longer exist in the stock with the required quantity",
                    UserId = cart.CustomerId

                };

                _notificationRepository.SendNotification(notification);

                await _notificationRepository.SaveChanges();

                _context.Carts.Remove(cart);
            }
        }

        public async Task<Product> GetProductAsync(int id, string sellerId)
        {
            return await _context.Products.Include(p => p.ProductPhotos)
                .FirstOrDefaultAsync(pro => pro.Id == id &&  pro.SellerId == sellerId);
        }

        public async Task<IEnumerable<MessageDto>> GetProductMessages(int productId, string username)
        {
            return await _context.Messages.Where(m => m.LastMessage && m.ProductId == productId)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<PagedList<ProductCardDto>> GetProductsAsync(string sellerId, PaginationParams paginationParams)
        {
            var query = _context.Products.Where(pro => pro.SellerId == sellerId && pro.State == "active").AsQueryable();

            return await PagedList<ProductCardDto>.CreateAsync(
                query.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<PagedList<PaymentDto>> GetPayments(string sellerId, PaginationParams paginationParams)
        {
            var query = _context.Sales
                .Where(sales => sales.SellerId == sellerId).AsQueryable();

            return await PagedList<PaymentDto>.CreateAsync(
                query.ProjectTo<PaymentDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<int> GetShopViews(string shopname)
        {
            var views = await _context.Products
                .Where(product => product.SellerName == shopname)
                .Select(product => product.ProductViews)
                .ToArrayAsync();

            return views.Sum();
        }
    }
}
