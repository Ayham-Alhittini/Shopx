using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopx.API.Data;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Extensions;
using Shopx.API.Helper;
using Shopx.API.Helper.Filter_Params;
using Shopx.API.Helper.Stripe;
using Shopx.API.Interfaces;
using Stripe;
using System.ComponentModel.DataAnnotations;

namespace Shopx.API.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : BaseApiController
    {
        private readonly IStripeAppService _stripeService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWishListRepository _wishListRepository;
        private readonly DataContext _context;

        public CustomerController(ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IMapper mapper,
            IUserRepository userRepository,
            IStripeAppService stripeService,
            UserManager<AppUser> userManager,
            IWishListRepository wishListRepository,
            DataContext context)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _stripeService = stripeService;
            _userManager = userManager;
            _wishListRepository = wishListRepository;
            _context = context;
        }

        [HttpPost("fill-guest-cart")]
        public async Task<ActionResult> FillGuestCart(AddToCartDto[] carts)
        {
            foreach(var cart in carts)
            {
                await AddToCart(cart);
            }
            return Ok();
        }

        [HttpPost("checkout")]
        public async Task<ActionResult> Checkout([FromBody]CreateOrderDto orderDto, CancellationToken ct)
        {
            var cartProducts = await _customerRepository.GetCarts(User.GetUserId());

            if (cartProducts.Count == 0)
                return BadRequest("cart empty!");


            ////check that 
            ///each product exist
            ///each quantity exist
            ///no quantity less than 1
            var products = new List<Entities.Product>();

            foreach (var cartItem in cartProducts)
            {
                var product = cartItem.Product;
                if (product == null)
                    return NotFound($"product with id {cartItem.ProductId} not found");
                if (cartItem.Quantity > product.Quantity)
                    return NotFound($"only {product.Quantity} exist in the stock, for product {product.Id}");
                if (cartItem.Quantity < 1)
                    return BadRequest($"quantity can't be less than 1, bad request found for product with id {product.Id}");

                products.Add(product);
            }


            var knownAs = await _userRepository.GetKnownAs(User.GetUserId());



            ///create customer account on stripe

            AddStripeCustomer customer = new AddStripeCustomer(User.GetEmail(), knownAs,
                    new AddStripeCard(knownAs, orderDto.Card.CardNumber, orderDto.Card.ExpirationYear, orderDto.Card.ExpirationMonth, orderDto.Card.Cvc)
                );

            StripeCustomer createdCustomer = await _stripeService.AddStripeCustomerAsync(
                customer,
                ct);
            

            ///add address
            var address = _mapper.Map<Entities.Address>(orderDto.Address);

            _customerRepository.AddAddress(address);

            await _customerRepository.SaveAllAsync();



            ///add order
            var order = new Order
            {
                CustomerId = User.GetUserId(),
                CustomerKnownAs = knownAs,
                AddressId = address.Id,
                NumberOfProducts = cartProducts.Sum(cart => cart.Quantity),
                DeliveryOption = orderDto.DeliveryOption,
                DeliveryPrice = GetDeliveryPrice(orderDto.DeliveryOption),
            };

            order.Total = cartProducts.Sum(cart => cart.Total) + order.DeliveryPrice;

            _customerRepository.AddOrder(order);

            await _customerRepository.SaveAllAsync();

            ///add sales
            var userId = User.GetUserId();
            var username = User.GetUsername();
            var orderId = order.Id;

            foreach (var cartItem in cartProducts)
            {
                var sales = _mapper.Map<Sales>(cartItem);
                sales.OrderId = orderId;
                ///perform buy operation
                _customerRepository.buy(sales);

                await _customerRepository.SaveAllAsync();

                string description = $"Customer: {knownAs} | Seller: {sales.SellerName} | Sales Id: {sales.Id}";

                AddStripePayment payment =
                    new AddStripePayment(createdCustomer.CustomerId, description, "usd", Convert.ToInt64(sales.Total * 100));


                await _stripeService.AddStripePaymentAsync(payment,ct);
            }

            



            ///decrease quantity of taken products
            for (int i = 0; i < cartProducts.Count; ++i)
            {
                products[i].Quantity -= cartProducts[i].Quantity;
            }
            ///clear cart items for this customer

            _customerRepository.ClearCart(User.GetUserId());

            if (await _customerRepository.SaveAllAsync())
                return Ok();

            return BadRequest("Something went wrong during checkout!");
        }

        [HttpPost("report")]
        public async Task<ActionResult> Report(CreateReportDto reportDto)
        {
            var product = await _productRepository.GetProductByIdAsync(reportDto.ProductId);
            if (product == null)
                return NotFound("Product not exist");


            ///check if already reported about this product
            var _report = await _context.Reports
                .Where(report => report.CustomerId == User.GetUserId() && report.ProductId == reportDto.ProductId)
                .FirstOrDefaultAsync();

            if (_report != null)
                return BadRequest("You already report this product");

            var report = new ReportProduct
            {
                CustomerId = User.GetUserId(),
                ProductId = reportDto.ProductId,
                ReportReason = reportDto.ReportReason,
            };

            if (report.ReportReason == "Other")
            {
                report.ReportDetails = reportDto.ReportDetails;
            }

            _context.Reports.Add(report);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("my-orders")]
        public async Task<ActionResult> MyOrders([FromQuery]PaginationParams paginationParams)
        {
            var orders = await _customerRepository.myOrders(User.GetUserId(), paginationParams);


            Response.AddPaginationHeader(new PaginationHeader(orders.CurrentPage, orders.PageSize, orders.TotalCount, orders.TotalPages));

            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        [HttpPut("change-phone-number")]
        public async Task<ActionResult> ChangePhoneNumber([Required][MinLength(12)][FromQuery]string PhoneNumber)
        {
            var customer = await _userRepository.GetUserByIdAsync(User.GetUserId());

            ///make validation for phone number if it's changed

            PhoneNumber = GenericMethod.GetPhoneNumberFormat(PhoneNumber);


            if (customer.PhoneNumber != PhoneNumber)
            {

                ///check if phone is taken by other user
                if (await PhoneNumberIsTaken(PhoneNumber))
                {
                    return BadRequest("Phone number is taken for different user");
                }

                ///check if phone number is real or fake
                if (!GenericMethod.CheckMobileNumber(PhoneNumber))
                {
                    return BadRequest("Invalid phone number");
                }

                customer.PhoneNumber = PhoneNumber;

                await _userRepository.SaveAllAsync();
            }

            return Ok();
        }

        [HttpGet("get-phone-number")]
        public async Task<ActionResult> GetPhoneNumber()
        {
            var phone = await _userManager.Users
                .Where(u => u.Id == User.GetUserId())
                .Select(u => u.PhoneNumber)
                .FirstOrDefaultAsync();

            return Ok(new
            {
                PhoneNumber = phone,
            });
        }

        [HttpGet("get-order-details/{id}")]
        public async Task<ActionResult> GetOrderDetails(int id)
        {
            var order = await _customerRepository.GetOrderDetails(User.GetUserId(), id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost("add-to-cart")]
        public async Task<ActionResult> AddToCart(AddToCartDto addToCart)
        {
            var product = await _productRepository.GetProductByIdAsync(addToCart.ProductId);
            if (product == null || product.State != States.active)
            {
                return NotFound();
            }
            ///silly requests
            if (addToCart.Quantity == 0) 
            {
                return BadRequest("quantity should be greater than zero");
            }
            ////no enough product to buy
            if (addToCart.Quantity > product.Quantity)
            {
                return BadRequest($"There is only {product.Quantity} left in the stock");
            }
            //
            var cart = await _customerRepository.GetCart(User.GetUserId(), product.Id);

            if (cart == null)
            {
                var carts = await _customerRepository.GetCarts(User.GetUserId());
                if (carts.Count + 1 > 25)
                    return BadRequest("Different Product Limit Size Is 100");
                cart = new ShoppingCart()
                {
                    CustomerId = User.GetUserId(),
                    CustomerUsername = User.GetUsername(),
                    SellerId = product.SellerId,
                    SellerName = product.SellerName,
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    Price = product.Price - product.Price * product.DiscountRate / 100.0,
                    Quantity = addToCart.Quantity,
                };
                cart.Total = cart.Price * cart.Quantity;
                _customerRepository.AddToCart(cart);
            }
            else
            {
                cart.Quantity = addToCart.Quantity;
                cart.Total = cart.Price * cart.Quantity;
            }


            await _customerRepository.SaveAllAsync();

            return Ok();
        }

        [HttpDelete("delete-from-cart/{productId}")]
        public async Task<ActionResult> RemoveFromCart(int productId)
        {
            var cart = await _customerRepository.GetCart(User.GetUserId(), productId);
            if (cart == null)
            {
                return NotFound();
            }
            _customerRepository.RemoveFromCart(cart);

            if (await _customerRepository.SaveAllAsync())
            {
                return Ok(new
                {
                    deletedQuantity = cart.Quantity,
                });
            }
            return BadRequest("Something went wrong during deleteing");
        }

        [HttpGet("my-cart")]
        public async Task<ActionResult> GetCarts()
        {
            var carts = await _customerRepository.GetCarts(User.GetUserId());

            var result = _mapper.Map<IEnumerable<CartDto>>(carts);

            InitializationCustomer(ref result, await _wishListRepository.GetWishListsAsync(User.GetUserId()));
            

            return Ok(result);
        }

        //////
        private double GetDeliveryPrice(string deliveryOption)
        {
            if (deliveryOption == DeliveryOptions.Quick)
                return 10.0;
            else if (deliveryOption == DeliveryOptions.Medium)
                return 5.0;
            else if (deliveryOption == DeliveryOptions.Slow)
                return 2.0;
            else if (deliveryOption == DeliveryOptions.Free)
                return 0.0;
            else
                throw new ArgumentException($"{deliveryOption} is not valid");
        }
        private async Task<bool> PhoneNumberIsTaken(string phoneNumber)
        {
            return await _userManager.Users.AnyAsync(user => user.PhoneNumber == phoneNumber);
        }
        private void InitializationCustomer(ref IEnumerable<CartDto> carts, List<WishList> wishes)
        {
            Dictionary<int, bool> existOnWishes = new Dictionary<int, bool>();

            foreach (var wish in wishes)
            {
                existOnWishes[wish.ProductId] = true;
            }

            foreach (var cart in carts)
            {
                cart.Product.OnCart = true;
                if (existOnWishes.ContainsKey(cart.Product.Id))
                {
                    cart.Product.OnWishlist = true;
                }
            }
        }
    }
}
