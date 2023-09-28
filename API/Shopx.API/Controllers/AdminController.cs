using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shopx.API.Data;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Extensions;
using Shopx.API.Helper;
using Shopx.API.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Shopx.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly DataContext _context;
        public AdminController(
            UserManager<AppUser> userManager,
            IMapper mapper, DataContext context, 
            IProductRepository productRepository,
            IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _productRepository = productRepository;
            _emailService = emailService;
        }

        [HttpGet("get-active-users")]
        public async Task<ActionResult> GetActiveUsers([FromQuery] PaginationParams paginationParams, string AccountType = "Seller")
        {
            var users = _userManager.Users
                .Where(u => u.AccountState == States.active && u.AccountType == AccountType)
                .Include(u => u.BackgroundPhoto);

            var result = await PagedList<UserDto>
                .CreateAsync(users.ProjectTo<UserDto>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }

        [HttpGet("get-banned-users")]
        public async Task<ActionResult> GetBannedUsers([FromQuery] PaginationParams paginationParams, string AccountType = "Seller")
        {
            var users = _userManager.Users
                .Where(u => u.AccountState == States.banned && u.AccountType == AccountType)
                .Include(u => u.BackgroundPhoto);

            var result = await PagedList<UserDto>
                .CreateAsync(users.ProjectTo<UserDto>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }

        

        [HttpGet("get-products")]
        public async Task<ActionResult> GetProducts([FromQuery] ManageProductParams manageProduct)
        {
            var products = _context.Products.Include(p => p.ReportProducts)
                .OrderByDescending(p => p.ReportProducts.Count())
                .AsQueryable();

            ///product state
            if (manageProduct.ProductState == States.active)
            {
                products = products.Where(pro => pro.State != States.banned);
            }
            else
            {
                products = products.Where(pro => pro.State == States.banned);
            }
            ///if product seller not null find the product for that seller
            if (!string.IsNullOrEmpty(manageProduct.SellerName))
            {
                products = products.Where(product => product.SellerName == manageProduct.SellerName);
            }


            var result = await PagedList<ProductCardDto>
            .CreateAsync(products.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider), manageProduct.PageNumber, manageProduct.PageSize);


            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }

        [HttpGet("get-shops-request")]
        public async Task<ActionResult> GetShopsRequest([FromQuery] PaginationParams paginationParams)
        {
            var users = _userManager.Users
                .Where(u => u.AccountState == States.pending)
                .Include(u => u.BackgroundPhoto)
                .OrderByDescending(u => u.Created);

            var result = await PagedList<SellerRequestDto>
                .CreateAsync(users.ProjectTo<SellerRequestDto>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }

        
        [HttpGet("get-product-reports/{productId}")]
        public async Task<ActionResult>  GetProductReports(int productId)
        {
            var reports = await _context.Reports
                .Where(r => r.ProductId == productId)
                .Include(r => r.Customer).ThenInclude(b => b.BackgroundPhoto)
                .OrderBy(r => r.WatchDate)
                .ThenBy(r => r.SendDate)
                .ToListAsync();

            ///mark them as watched

            return Ok(_mapper.Map<IEnumerable<ReportDto>>(reports));
        }

        [HttpPut("watch/{id}")]
        public async Task<ActionResult>  Watch(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            report.WatchDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok();
        }



        [HttpGet("get-user/{userInfo}")]
        public async Task<ActionResult> GetUser(string userInfo)
        {
            var user = await getUser(userInfo);
            if (user == null)
            {
                return NotFound("User not exist");
            }
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpGet("get-product/{productId}")]
        public async Task<ActionResult> GetProduct(int productId)
        {
            var product = _context.Products.Where(pro => pro.Id == productId);

            return Ok(await product.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync());
        }

        [HttpPut("accept-shops/{userInfo}")]
        public async Task<ActionResult> AcceptShop(string userInfo)
        {
            var user = await getUser(userInfo);

            if (user == null) return NotFound("User not exist");

            if (user.AccountState != States.pending) return BadRequest("request performed previously");

            user.AccountState = States.active;

            await _context.SaveChangesAsync();

            ///Send email to notify user

            var messageContent = "The admin accept your shop request you can login now and post your products";

            var message = new EmailMessage(new string[] { user.Email! }, "Request Accepted", messageContent);
            _emailService.SendEmail(message);

            return Ok(new
            {
                Response = "Shops Accepted Successfully"
            });
        }

        [HttpPut("reject-shops/{userInfo}")]
        public async Task<ActionResult> RejectShop(string userInfo,[FromQuery] [Required] string rejectionReason)
        {
            var user = await getUser(userInfo);

            if (user == null) return NotFound("User not exist");

            if (user.AccountState != States.pending) return BadRequest("request performed previously");

            user.AccountState = States.rejected;

            await _context.SaveChangesAsync();


            var messageContent = "Your request to open shop on Shopx.com is rejected by the admin , admin reason: " + rejectionReason;

            ///Send email to notify user
            var message = new EmailMessage(new string[] { user.Email! }, "Request Rejected", messageContent);
            _emailService.SendEmail(message);

            return Ok(new
            {
                Response = "Shops Rejected Successfully"
            });
        }

        [HttpPut("block-user/{userInfo}")]
        public async Task<ActionResult> BlockUser(string userInfo, [FromQuery] bool blockCommand = true)
        {
            ///accept many search field 
            ///could get user by 
            ///1-UserName
            ///2-Email
            ///3-UserId
           
            var user = await getUser(userInfo);
            if (user == null)
            {
                return NotFound("User not exist");
            }

            if (await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return BadRequest("Admin can not be banned");
            }

            user.AccountState = blockCommand ? States.banned : States.active;

            if (await _userManager.IsInRoleAsync(user, RoleNames.Seller))
            {
                /*
                if the user (You want to banned) is seller then mark all his product state 
                that are active to be temp-banned so customer won't see products that his owner is 
                banned.
                 */

                var sellerProducts = await _context.Products
                    .Where(pro => pro.SellerId == user.Id)
                    .ToListAsync();

                if (blockCommand)
                {
                    foreach (var product in sellerProducts) if (product.State == States.active)
                            product.State = States.temp_banned;
                }
                else
                {
                    foreach (var product in sellerProducts) if (product.State == States.temp_banned)
                            product.State = States.active;
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { Response = user.AccountState == States.banned ? $"user who is known as {user.KnownAs} is banned successfully" :
                $"user who is known as {user.KnownAs} is activated successfully"
            });
        }

        [HttpPut("block-product/{productId}")]
        public async Task<ActionResult> BlockProduct(int productId, [FromQuery] bool blockCommand = true, string reason = "unedified")
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product == null)
                return NotFound("Product not exist");

            var productSeller = await _context.Users.FindAsync(product.SellerId);

            if (productSeller.AccountState == States.active)
                product.State = blockCommand ? States.banned : States.active;
            else
                product.State = blockCommand ? States.banned : States.temp_banned;


            ///send notification to the seller that the product being banned or activated
            ///notification will be send whether state active or banned

            string _state = product.State == States.active ? "activated" : "banned";
            string description = $"The Product with id {product.Id}, is being {_state}, and that for : {reason}";
            var notification = new Notification
            {
                Title = $"Your product state is modified by the admin" ,
                Description = description,
                UserId = productSeller.Id,
            };

            _context.Notifications.Add(notification);

            await _productRepository.SaveAllAsync();

            return Ok(new
            {
                Response = product.State == States.banned ? "Product banned successfully" : "Product activated successfully"
            });
        }

        ///private methods
        private async Task<AppUser> getUser(string userInfo)
        {
            var user = await _userManager.Users
                .Include(u => u.BackgroundPhoto)
                .Where(u => u.Id == userInfo || u.UserName == userInfo || u.Email == userInfo)
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
