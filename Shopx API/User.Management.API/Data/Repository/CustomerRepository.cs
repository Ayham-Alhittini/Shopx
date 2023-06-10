using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using Microsoft.EntityFrameworkCore;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Helper;
using Shopx.API.Helper.Filter_Params;
using Shopx.API.Helper.Stripe;
using Shopx.API.Interfaces;
using Stripe;

namespace Shopx.API.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CustomerRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        

        public void buy(Sales sale)
        {
            _context.Sales.Add(sale);
        }


        public void AddToCart(ShoppingCart cart)
        {
            _context.Carts.Add(cart);
        }


        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ShoppingCart>> GetCarts(string customerId)
        {
            return await _context.Carts.Include(c => c.Product).ThenInclude(p => p.ProductPhotos)
                .Where(c => c.CustomerId == customerId && c.Product.State == "active")
                .Take(25)
                .ToListAsync();
        }

        public void AddAddress(Shopx.API.Entities.Address address)
        {
            _context.Addresses.Add(address);
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
        }

        public void ClearCart(string customerId)
        {
            var carts = _context.Carts
                .Where(cart => cart.CustomerId == customerId).ToList();

            foreach (var cart in carts)
            {
                _context.Remove(cart);
            }
        }

        public async Task<ShoppingCart> GetCart(string customerId, int productId)
        {
            return await _context.Carts.FindAsync(customerId, productId);
        }

        public void RemoveFromCart(ShoppingCart cart)
        {
            _context.Carts.Remove(cart);
        }

        public async Task<PagedList<Order>> myOrders(string customerId, PaginationParams paginationParams)
        {
            var orders = _context.Orders.Where(order => order.CustomerId == customerId);

            return await PagedList<Order>
                .CreateAsync(orders,paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<OrderDto> GetOrderDetails(string customerId, int orderId)
        {
            var order =  _context.Orders
                .Where(order => order.Id == orderId && order.CustomerId == customerId);

            return await order
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<bool> IsOnCart(string customerId, int productId)
        {
            return await _context.Carts
                .Where(c => c.CustomerId == customerId && c.ProductId == productId)
                .AnyAsync();
        }
    }
}
