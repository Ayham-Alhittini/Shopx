using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Shopx.API.Data;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Extensions;
using Shopx.API.Helper;
using Shopx.API.Helper.Filter_Params;
using Shopx.API.Interfaces;
using Stripe;
using System.ComponentModel.DataAnnotations;
    
namespace Shopx.API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IBrowseHistoryRepository _browseHistoryRepository;
        private readonly DataContext _context;
        private readonly ISellerRepository _sellerRepository;
        private readonly IWishListRepository _wishListRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<AppUser> _userManager;

        public UserController(IUserRepository userRepository,
            IPhotoService photoService, IMapper mapper,
            IProductRepository productRepository,
            IReviewRepository reviewRepository,
            IBrowseHistoryRepository browseHistoryRepository,
            UserManager<AppUser> userManager,
            ISellerRepository sellerRepository,
            IWishListRepository wishListRepository,
            ICustomerRepository customerRepository,
            DataContext context)
        {
            _userRepository = userRepository;
            _photoService = photoService;
            _mapper = mapper;
            _productRepository = productRepository;
            _reviewRepository = reviewRepository;
            _browseHistoryRepository = browseHistoryRepository;
            _userManager = userManager;
            _sellerRepository = sellerRepository;
            _wishListRepository = wishListRepository;
            _customerRepository = customerRepository;
            _context = context;
        }

        [HttpGet("get-shops")]
        public async Task<ActionResult> GetShops([FromQuery]PaginationParams paginationParams)
        {
            var shops = _context.Users
                .Where(u => u.AccountType == "Seller")
                .Include(u => u.ShopReviews)
                .AsQueryable();

            if (User.GetAccountType() != "Admin")
            {
                shops = shops.Where(s => s.AccountState == States.active);
            }


            var result = await PagedList<SellerCardDto>
                .CreateAsync(shops.ProjectTo<SellerCardDto>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);


            foreach (var shop in result)
            {
                var reviews = await _context.ShopReviews
                    .Where(r => r.SellerId == shop.Id)
                    .Select(r => r.RatingValue)
                    .ToListAsync();


                if (reviews.Count > 0)
                {
                    shop.ShopRate = Convert.ToDouble(reviews.Sum()) / reviews.Count;

                    int firstDecimal = (int)((shop.ShopRate - (int)shop.ShopRate) * 10);
                    shop.ShopRate = (int)shop.ShopRate + (double)firstDecimal / 10.0;
                }
                else
                {
                    shop.ShopRate = 0.0;
                }
            }

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }

        [Authorize]
        [HttpPut("change-background")]
        public async Task<ActionResult<PhotoDto>> AddBackground([Required]IFormFile file)
        {
            /*
             if already there a background
                -remove background from cloud
                -remove background for this user from backgrounds table (because only one allwoed)
                -add the new background

            if not
                -add the new background
             */


            var user = await _userRepository.GetUserByNameAsync(User.GetUsername());

            if (user.BackgroundPhoto != null)
            {
                ///delete from cloud
                if (user.BackgroundPhoto.PublicId != null)
                {
                    var deleteResult = await _photoService.DeletePhotoAsync(user.BackgroundPhoto.PublicId);

                    if (deleteResult.Error != null)
                    {
                        return BadRequest(deleteResult.Error.Message);
                    }
                }

                ///delete from background table
                _context.Backgrounds.Remove(user.BackgroundPhoto);

                if (await _context.SaveChangesAsync() == 0)
                {
                    return BadRequest("Background not deleted from backgrounds table");
                }
            }


            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }


            
            var photo = new BackgroundPhoto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            user.BackgroundPhoto = photo;

            if (await _userRepository.SaveAllAsync())
            {
                return _mapper.Map<PhotoDto>(photo);
            }
            return BadRequest("Can't Add Photo");
        }


        [HttpGet("get-shop/{shopname}")]
        public async Task<ActionResult> GetSeller(string shopname)
        {
            var seller = await _userRepository.GetSeller(shopname);

            if (seller == null)
                return NotFound("Shop not exist");

            var shop = await _userManager.FindByNameAsync(shopname);
            seller.ShopViews = await _sellerRepository.GetShopViews(shopname);

            var reviews = await _reviewRepository.GetShopReviews(shop.Id);

            if (User.GetUserId() != null)
            {
                foreach (var review in reviews)
                {
                    var vote = await _reviewRepository.ShopReviewVote(User.GetUserId(), review.Id);

                    review.Initial = vote == null ? 0 : vote.VoteValue;
                }
            }


            if (reviews.Count > 0)
            {
                ShopReviewDetails reviewDetails = new ShopReviewDetails
                {
                    Reviews = reviews,
                    NumberOfReviews = reviews.Count,
                    FiveStarCount = reviews.Where(r => r.RatingValue == 5).Count(),
                    FourStarCount = reviews.Where(r => r.RatingValue == 4).Count(),
                    ThreeStarCount = reviews.Where(r => r.RatingValue == 3).Count(),
                    TwoStarCount = reviews.Where(r => r.RatingValue == 2).Count(),
                    OneStarCount = reviews.Where(r => r.RatingValue == 1).Count(),
                };
                reviewDetails.ShopRate = reviews.Sum(r => r.RatingValue) / Convert.ToDouble(reviews.Count());

                int firstDecimal = (int)((reviewDetails.ShopRate - (int)reviewDetails.ShopRate) * 10);
                reviewDetails.ShopRate = (int)reviewDetails.ShopRate + (double)firstDecimal / 10.0;

                reviewDetails.FiveStarPercentage = reviewDetails.FiveStarCount / Convert.ToDouble(reviews.Count()) * 100.0;
                reviewDetails.FourStarPercentage = reviewDetails.FourStarCount / Convert.ToDouble(reviews.Count()) * 100.0;
                reviewDetails.ThreeStarPercentage = reviewDetails.ThreeStarCount / Convert.ToDouble(reviews.Count()) * 100.0;
                reviewDetails.TwoStarPercentage = reviewDetails.TwoStarCount / Convert.ToDouble(reviews.Count()) * 100.0;
                reviewDetails.OneStarPercentage = reviewDetails.OneStarCount / Convert.ToDouble(reviews.Count()) * 100.0;

                seller.ShopReview = reviewDetails;
            }
            else
            {
                seller.ShopReview = new ShopReviewDetails();
            }

            return Ok(seller);
        }

        [HttpGet("get-shop-products/{shopname}")]
        public async Task<ActionResult> GetShopProducts(string shopname, [FromQuery] PaginationParams paginationParams)
        {
            var products = _context.Products
                .Where(p => p.SellerName == shopname && p.State == States.active)
                .AsQueryable();

            var result = await PagedList<ProductCardDto>
                .CreateAsync(products.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }

        [HttpGet("get-customer/{customerId}")]
        public async Task<ActionResult> GetCustomer(string customerId)
        {
            var customer = await _userRepository.GetCustomer(customerId);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }


        [HttpGet("get-product/{productId}")]
        public async Task<ActionResult> GetProduct(int productId)
        {
            var product = await _productRepository.GetProductDtoAsync(productId, User.GetAccountType());
            if (product == null)
                return NotFound("Product not exist");

            var reviews = await _reviewRepository.GetProductReviews(productId);

            if (User.GetUserId() != null)
            {
                foreach (var review in reviews)
                {
                    var vote = await _reviewRepository.ProductReviewVote(User.GetUserId(), review.Id);

                    review.Initial = vote == null ? 0 : vote.VoteValue;
                }
            }


            if (reviews.Count > 0)
            {
                var reviewDetails = new ProductReviewDetails
                {
                    ProductReviews = reviews,
                    NumberOfReviews = reviews.Count(),
                    FiveStarCount = reviews.Where(review => review.RatingValue == 5).Count(),
                    FourStarCount = reviews.Where(review => review.RatingValue == 4).Count(),
                    ThreeStarCount = reviews.Where(review => review.RatingValue == 3).Count(),
                    TwoStarCount = reviews.Where(review => review.RatingValue == 2).Count(),
                    OneStarCount = reviews.Where(review => review.RatingValue == 1).Count(),
                };
                reviewDetails.ProductRate = reviews.Sum(review => review.RatingValue) / Convert.ToDouble(reviews.Count());

                int firstDecimal = (int)((reviewDetails.ProductRate - (int)reviewDetails.ProductRate) * 10);
                reviewDetails.ProductRate = (int)reviewDetails.ProductRate + firstDecimal / 10.0;

                reviewDetails.FiveStarPercentage = reviewDetails.FiveStarCount / Convert.ToDouble(reviews.Count()) * 100.0;
                reviewDetails.FourStarPercentage = reviewDetails.FourStarCount / Convert.ToDouble(reviews.Count()) * 100.0;
                reviewDetails.ThreeStarPercentage = reviewDetails.ThreeStarCount / Convert.ToDouble(reviews.Count()) * 100.0;
                reviewDetails.TwoStarPercentage = reviewDetails.TwoStarCount / Convert.ToDouble(reviews.Count()) * 100.0;
                reviewDetails.OneStarPercentage = reviewDetails.OneStarCount / Convert.ToDouble(reviews.Count()) * 100.0;

                product.ProductReview = reviewDetails;
            }
            else
            {
                product.ProductReview = new ProductReviewDetails();
            }

            /// since the product will be send to the customer it's should on his browse history
            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);


            if (userId != null && user.AccountType == "Customer")
            {
                ///this method will be active when user authenticated if user not authenticated he could see the product 
                ///without add it to the browse history

                var browse = await _browseHistoryRepository.GetBrowseHistory(userId, productId);
                if (browse != null)
                {
                    ///shown before 
                    ///just update the browse date
                    browse.BrowseDate = DateTime.UtcNow;
                }
                else
                {
                    ///not shown before (not on history)
                    browse = new BrowseHistory
                    {
                        CustomerId = userId,
                        ProductId = productId,
                    };

                    ///add views to product
                    var productDb = await _productRepository.GetProductByIdAsync(productId);

                    productDb.ProductViews++;
                    

                    _browseHistoryRepository.Browse(browse);

                }

                ////get initial value for product (is it on wish list) & (is it on cart)

                product.OnCart = await _context.Carts.Where(c => c.CustomerId == User.GetUserId() && c.ProductId == productId).FirstOrDefaultAsync() != null;
                product.OnWishlist = await _wishListRepository.GetWishListAsync(User.GetUserId(), productId) != null;
                product.Reported =  await _context.Reports
                     .Where(report => report.CustomerId == User.GetUserId() && report.ProductId == productId)
                    .FirstOrDefaultAsync() != null;


                await _browseHistoryRepository.SaveChangesAsync();
            }


            return Ok(product);
        }

        ///////////////////////filters

        [HttpGet("generic-filter")]
        public async Task<ActionResult> GetProducts([FromQuery] GenericParams genericParams)
        {
            var products = _context.Products.Where(pro => pro.State == States.active).AsQueryable().AsNoTracking();
            genericParams.SearchContent = genericParams.SearchContent?.ToLower();

            products = GetCommonFilter(products, genericParams.SearchContent, genericParams.MinPrice, genericParams.MaxPrice);

            if (!genericParams.Category.IsNullOrEmpty())
            {
                products = products.Where(pro => pro.Category == genericParams.Category);
            }

            var result = await PagedList<ProductCardDto>
                .CreateAsync(products.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider), genericParams.PageNumber, genericParams.PageSize);

            if (IsCustomer())
            {
                InitializationCustomer(ref result,
                    await _customerRepository.GetCarts(User.GetUserId()),
                    await _wishListRepository.GetWishListsAsync(User.GetUserId()));
            }

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage,result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }

        [HttpGet("computers&laptops-filter")]
        public async Task<ActionResult> GetLaptops([FromQuery]LaptopParams laptopParams)
        {
            var products = _context.Products.Where(pro => pro.State == States.active).AsQueryable();

            products = products.Where(pro => pro.Category == Categories.ComputersAndLaptops);

            if (!laptopParams.Type.IsNullOrEmpty())
                products = products.Where(pro => pro.SubCategory == laptopParams.Type);

            products = GetCommonFilter(products, laptopParams.SearchContent, laptopParams.MinPrice, laptopParams.MaxPrice);

            ///laptop filter

            if (laptopParams.Brand.Count > 0)
                products = products.Where(pro => laptopParams.Brand.Contains(pro.Laptop.Brand));
            if (laptopParams.OperatingSystem.Count > 0)
                products = products.Where(pro => laptopParams.OperatingSystem.Contains(pro.Laptop.OperatingSystem));
            if (laptopParams.ScreenSize.Count > 0)
                products = products.Where(pro => laptopParams.ScreenSize.Contains(pro.Laptop.ScreenSize));
            if (laptopParams.Ram.Count > 0)
                products = products.Where(pro => laptopParams.Ram.Contains(pro.Laptop.Ram));

            var result = await PagedList<ProductCardDto>
                 .CreateAsync(products.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider), laptopParams.PageNumber, laptopParams.PageSize);

            if (IsCustomer())
            {
                InitializationCustomer(ref result,
                    await _customerRepository.GetCarts(User.GetUserId()),
                    await _wishListRepository.GetWishListsAsync(User.GetUserId()));
            }

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }

        [HttpGet("vehicles-filter")]
        public async Task<ActionResult> GetVehicles([FromQuery]VehicleParams vehicleParams)
        {
            var products = _context.Products.Where(pro => pro.Category == Categories.Vehicles && pro.State == States.active).AsQueryable().AsNoTracking();

            products = GetCommonFilter(products, vehicleParams.SearchContent, vehicleParams.MinPrice, vehicleParams.MaxPrice);

            products = products.Where(pro => pro.Vehicle.Year >= vehicleParams.YearFrom && pro.Vehicle.Year <= vehicleParams.YearTo);

            if (vehicleParams.CarMake.Count > 0)
                products = products.Where(pro => vehicleParams.CarMake.Contains(pro.Vehicle.CarMake));
            if (vehicleParams.Model.Count > 0)
                products = products.Where(pro => vehicleParams.Model.Contains(pro.Vehicle.Model));
            if (vehicleParams.Type.Count > 0)
                products = products.Where(pro => vehicleParams.Type.Contains(pro.Vehicle.Type));
            if (vehicleParams.Transmission.Count > 0)
                products = products.Where(pro => vehicleParams.Transmission.Contains(pro.Vehicle.Transmission));
            if (vehicleParams.Fuel.Count > 0)
                products = products.Where(pro => vehicleParams.Fuel.Contains(pro.Vehicle.Fuel));
            if (vehicleParams.Color.Count > 0)
                products = products.Where(pro => vehicleParams.Color.Contains(pro.Vehicle.Color));
            if (vehicleParams.Condition.Count > 0)
                products = products.Where(pro => vehicleParams.Condition.Contains(pro.Vehicle.Condition));
            if (vehicleParams.Kilometers.Count > 0)
                products = products.Where(pro => vehicleParams.Kilometers.Contains(pro.Vehicle.Kilometers));
            if (vehicleParams.Paint.Count > 0)
                products = products.Where(pro => vehicleParams.Paint.Contains(pro.Vehicle.Paint));

            var result = await PagedList<ProductCardDto>
                .CreateAsync(products.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider), vehicleParams.PageNumber, vehicleParams.PageSize);

            if (IsCustomer())
            {
                InitializationCustomer(ref result,
                    await _customerRepository.GetCarts(User.GetUserId()),
                    await _wishListRepository.GetWishListsAsync(User.GetUserId()));
            }

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }
        [HttpGet("pets-filter")]
        public async Task<ActionResult> GetPets([FromQuery]PetParams petParams)
        {
            var products = _context.Products.Where(pro => pro.Category == Categories.Pets && pro.State == States.active).AsQueryable().AsNoTracking();

            products = GetCommonFilter(products, petParams.SearchContent, petParams.MinPrice, petParams.MaxPrice);

            if (!petParams.PetName.IsNullOrEmpty())
            {
                products = products.Where(pro => pro.Pet.PetName == petParams.PetName);
                if (petParams.PetType.Count > 0)
                    products = products.Where(pro => petParams.PetType.Contains(pro.Pet.PetType));
            }


            var result = await PagedList<ProductCardDto>
                .CreateAsync(products.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider), petParams.PageNumber, petParams.PageSize);

            if (IsCustomer())
            {
                InitializationCustomer(ref result,
                    await _customerRepository.GetCarts(User.GetUserId()),
                    await _wishListRepository.GetWishListsAsync(User.GetUserId()));
            }

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }
        [HttpGet("mobile&tablets-filter")]
        public async Task<ActionResult> GetMobiles([FromQuery]MobileParams mobileParams)
        {
            var products = _context.Products.Where(pro => pro.Category == Categories.MobilesAndTablets && pro.State == States.active)
                .AsQueryable().AsNoTracking();

            products = GetCommonFilter(products, mobileParams.SearchContent, mobileParams.MinPrice, mobileParams.MaxPrice);

            if (!mobileParams.Type.IsNullOrEmpty())
                products = products.Where(pro => pro.Mobile.Type == mobileParams.Type);
            if (mobileParams.Brand.Count > 0)
            {
                products = products.Where(pro => mobileParams.Brand.Contains(pro.Mobile.Brand));

                if (mobileParams.Model.Count > 0)
                {
                    products = products.Where(pro => mobileParams.Model.Contains(pro.Mobile.Model));
                }
            }
            if (mobileParams.StorageSize.Count > 0)
                products = products.Where(pro => mobileParams.StorageSize.Contains(pro.Mobile.StorageSize));
            if (mobileParams.Color.Count > 0)
                products = products.Where(pro => mobileParams.Color.Contains(pro.Mobile.Color));
            if (mobileParams.ScreenSize?.Count > 0)
                products = products.Where(pro => mobileParams.ScreenSize.Contains(pro.Mobile.ScreenSize));

            var result = await PagedList<ProductCardDto>
                .CreateAsync(products.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider), mobileParams.PageNumber, mobileParams.PageSize);
            
            if (IsCustomer())
            {
                InitializationCustomer(ref result,
                    await _customerRepository.GetCarts(User.GetUserId()),
                    await _wishListRepository.GetWishListsAsync(User.GetUserId()));
            }

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }
        
        [HttpGet("accessories-filter")]
        public async Task<ActionResult> GetAccessories([FromQuery]AccessoriesParams accessoriesParams)
        {
            var products = _context.Products.Where(pro => pro.Category == accessoriesParams.Categoriy && pro.State == States.active)
                .AsQueryable().AsNoTracking();

            products = GetCommonFilter(products, accessoriesParams.SearchContent, accessoriesParams.MinPrice, accessoriesParams.MaxPrice);

            if (accessoriesParams.Type.Count > 0)
                products = products.Where(pro => accessoriesParams.Type.Contains(pro.Accessories.Type));

            var result = await PagedList<ProductCardDto>
                .CreateAsync(products.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider), accessoriesParams.PageNumber, accessoriesParams.PageSize);

            if (IsCustomer())
            {
                InitializationCustomer(ref result,
                    await _customerRepository.GetCarts(User.GetUserId()),
                    await _wishListRepository.GetWishListsAsync(User.GetUserId()));
            }

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }
        [HttpGet("monitors-filter")]
        public async Task<ActionResult> GetMonitors([FromQuery]MonitorParams monitorParams)
        {
            var products = _context.Products
                .Where(pro => pro.Category == Categories.ComputersAndLaptops && pro.SubCategory == SubCategories.Monitors && pro.State == States.active)
                .AsQueryable().AsNoTracking();

            products = GetCommonFilter(products, monitorParams.SearchContent, monitorParams.MinPrice, monitorParams.MaxPrice);

            if (monitorParams.Brand.Count > 0)
                products = products.Where(pro => monitorParams.Brand.Contains(pro.MonitorProduct.Brand));
            if (monitorParams.ScreenSize.Count > 0)
                products = products.Where(pro => monitorParams.ScreenSize.Contains(pro.MonitorProduct.ScreenSize));

            var result = await PagedList<ProductCardDto>
                .CreateAsync(products.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider), monitorParams.PageNumber, monitorParams.PageSize);

            if (IsCustomer())
            {
                InitializationCustomer(ref result,
                    await _customerRepository.GetCarts(User.GetUserId()),
                    await _wishListRepository.GetWishListsAsync(User.GetUserId()));
            }

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }

        //////private methods
        private IQueryable<Entities.Product> GetCommonFilter(IQueryable<Entities.Product> products
            , string SearchContent, double MinPrice, double MaxPrice)
        {
            if (!SearchContent.IsNullOrEmpty())
            {
                SearchContent = SearchContent?.Trim();
                products = products.Where(pro =>
                      pro.ProductName.Contains(SearchContent)
                    || SearchContent.Contains(pro.ProductName)
                    || SearchContent.Contains(pro.Category)
                    || SearchContent.Contains(pro.SubCategory)
                    || pro.ProductDescription.Contains(SearchContent)
                );
            }
            products = products.Where(pro => pro.Price >= MinPrice && pro.Price <= MaxPrice);
            return products.AsNoTracking();
        }
        private bool IsCustomer()
        {
            return User.GetAccountType() == "Customer";
        }

        private void InitializationCustomer(ref PagedList<ProductCardDto> products, List<ShoppingCart> carts, List<WishList> wishes)
        {
            Dictionary<int, bool> existOnCart = new Dictionary<int, bool>();
            Dictionary<int, bool> existOnWishes = new Dictionary<int, bool>();

            foreach(var cart in carts)
            {
                existOnCart[cart.ProductId] = true;
            }

            foreach(var wish in wishes)
            {
                existOnWishes[wish.ProductId] = true;
            }


            foreach(var product in products)
            {
                if (existOnCart.ContainsKey(product.Id))
                {
                    product.OnCart = true;
                }
                if (existOnWishes.ContainsKey(product.Id))
                {
                    product.OnWishlist = true;
                }
            }
        }

    }
}
