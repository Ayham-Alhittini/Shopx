using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shopx.API.Data;
using Shopx.API.DTOs;
using Shopx.API.DTOs.Create_Products;
using Shopx.API.Entities;
using Shopx.API.Entities.Product_Specification;
using Shopx.API.Extensions;
using Shopx.API.Helper;
using Shopx.API.Interfaces;
using Stripe;
using System.ComponentModel.DataAnnotations;

namespace Shopx.API.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;

        public SellerController(IProductRepository productRepository,
            ISellerRepository sellerRepository, IUserRepository userRepository,
            IPhotoService photoService, IMapper mapper,
            UserManager<AppUser> userManager,
            DataContext context)
        {
            _productRepository = productRepository;
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
            _photoService = photoService;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
        }



        [HttpGet("get-shop-customers")]
        public async Task<ActionResult> GetShopCustomers([FromQuery]PaginationParams paginationParams)
        {
            var customers = _context.Sales
                .Where(s => s.SellerId == User.GetUserId())
                .Select(s => s.Customer)
                .Distinct()
                .AsQueryable();


            

            var result = await PagedList<CustomerDto>
            .CreateAsync(customers.ProjectTo<CustomerDto>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }

        [HttpGet("get-product-statics")]
        public async Task<ActionResult> GetProductStatics()
        {
            var products = await _context.Products
                .Where(product => product.SellerId == User.GetUserId())
                .ToListAsync();

            var sales = await _context.Sales
                .Where(sale => sale.SellerId == User.GetUserId())
                .ToListAsync();

            var result = new List<ProductStaticDto>();

            for (int i = 0; i < products.Count; i++)
            {
                var productStatic = new ProductStaticDto
                {
                    ProductName = products[i].ProductName,
                    Id = products[i].Id,
                    Price = products[i].Price - products[i].Price * products[i].DiscountRate / 100.0,
                    State = products[i].State,
                    DiscountRate = products[i].DiscountRate,
                    OnStock = products[i].Quantity,
                    Views = products[i].ProductViews,
                    NumberOfCustomers = sales.Where(sale => sale.ProductId == products[i].Id)
                        .GroupBy(sale => sale.CustomerId)
                        .Count(),
                    SolidQuantity = sales.Where(sale => sale.ProductId == products[i].Id)
                        .Sum(sale => sale.Quantity),

                };
                productStatic.Total = productStatic.SolidQuantity * productStatic.Price;

                result.Add(productStatic);
            }



            return Ok(new
            {
                Total = result.Sum(r => r.Total),
                Statics = result
            });
        }


        ////////Adding Products///////////////////////////////////////////////////////////////////////////////////

        [HttpPost("add-product-computers&laptops")]
        public async Task<ActionResult> AddLaptop(CreateLaptopDto laptopDto)
        {
            ///convert it to laptop
            var laptop = _mapper.Map<LaptopAndComputer>(laptopDto);

            _productRepository.AddLaptop(laptop);

            if (await _productRepository.SaveAllAsync() == false)
            {
                return BadRequest(ModelState);
            }

            //convert it to product and add it to database

            var product = _mapper.Map<Entities.Product>(laptopDto);

            product.SellerId = User.GetUserId();
            product.SellerName = User.GetUsername();
            product.LaptopId = laptop.Id;
            product.Category = Categories.ComputersAndLaptops;
            product.SubCategory = laptopDto.Type;

            _productRepository.Add(product);

            if (await _productRepository.SaveAllAsync())
                return Ok(_mapper.Map<ProductDto>(product));

            return BadRequest("Something went wrong");
        }

        [HttpPost("add-product-vehicles")]
        public async Task<ActionResult> AddVehicle(CreateVehicleDto vehicleDto)
        {
            var vehicle = _mapper.Map<Vehicle>(vehicleDto);

            _productRepository.AddVehicle(vehicle);

            if (await _productRepository.SaveAllAsync() == false)
            {
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Entities.Product>(vehicleDto);

            product.SellerId = User.GetUserId();
            product.SellerName = User.GetUsername();
            product.VehicleId = vehicle.Id;
            product.Category = Categories.Vehicles;
            product.SubCategory = vehicleDto.Type;

            _productRepository.Add(product);

            if (await _productRepository.SaveAllAsync())
                return Ok(_mapper.Map<ProductDto>(product));

            return BadRequest("Something went wrong");
        }

        [HttpPost("add-product-pets")]
        public async Task<ActionResult> AddPet(CreatePetDto petDto)
        {
            var pet = _mapper.Map<Pet>(petDto);
            _context.Pets.Add(pet);
            if (await _context.SaveChangesAsync() == 0)
                return BadRequest("Something went wrong");

            var product = _mapper.Map<Entities.Product>(petDto);

            product.SellerId = User.GetUserId();
            product.SellerName = User.GetUsername();
            product.PetId = pet.Id;
            product.Category = Categories.Pets;
            product.SubCategory = petDto.PetName;


            _productRepository.Add(product);

            if (await _productRepository.SaveAllAsync())
                return Ok(_mapper.Map<ProductDto>(product));

            return BadRequest("Something went wrong");
        }

        [HttpPost("add-product-mobile&tablets")]
        public async Task<ActionResult> AddMobile(CreateMobilesDto mobileDto)
        {
            var mobile = _mapper.Map<Mobile>(mobileDto);
            _context.MobilesAndTablets.Add(mobile);
            if (await _context.SaveChangesAsync() == 0)
                return BadRequest("Something went wrong");

            var product = _mapper.Map<Entities.Product>(mobileDto);

            product.SellerId = User.GetUserId();
            product.SellerName = User.GetUsername();
            product.MobileId = mobile.Id;
            product.Category = Categories.MobilesAndTablets;
            product.SubCategory = mobileDto.Type;


            _productRepository.Add(product);

            if (await _productRepository.SaveAllAsync())
                return Ok(_mapper.Map<ProductDto>(product));

            return BadRequest("Something went wrong");
        }
        
        [HttpPost("add-product-accessories")]
        public async Task<ActionResult> AddAccessories(CreateAccessoriesDto accessoriesDto)
        {
            var accessorie = _mapper.Map<Accessories>(accessoriesDto);
            _context.Accessories.Add(accessorie);
            if (await _context.SaveChangesAsync() == 0)
                return BadRequest("Something went wrong");

            var product = _mapper.Map<Entities.Product>(accessoriesDto);

            product.SellerId = User.GetUserId();
            product.SellerName = User.GetUsername();
            product.AccessoriesId = accessorie.Id;
            product.Category = accessoriesDto.Category;
            product.SubCategory = SubCategories.Accessories;


            _productRepository.Add(product);

            if (await _productRepository.SaveAllAsync())
                return Ok(_mapper.Map<ProductDto>(product));

            return BadRequest("something went wrong");
        }

        [HttpPost("add-product-monitors")]
        public async Task<ActionResult> AddMonitor(CreateMonitorDto monitorDto)
        {
            var monitor = _mapper.Map<MonitorProduct>(monitorDto);
            _context.MonitorProducts.Add(monitor);
            if (await _context.SaveChangesAsync() == 0)
                return BadRequest("Something went wrong");

            var product = _mapper.Map<Entities.Product>(monitorDto);

            product.SellerId = User.GetUserId();
            product.SellerName = User.GetUsername();
            product.MonitorProductId = monitor.Id;
            product.Category = Categories.ComputersAndLaptops;
            product.SubCategory = SubCategories.Monitors;


            _productRepository.Add(product);

            if (await _productRepository.SaveAllAsync())
                return Ok(_mapper.Map<ProductDto>(product));

            return BadRequest("something went wrong");
        }






        ////////Editing Products//////////////////////////////////////////////////////////////////////////////////
        [HttpPut("edit-product-computers&laptops/{productId}")]
        public async Task<ActionResult> EditLaptop(int productId, CreateLaptopDto laptopDto)
        {
            var product = await _context.Products
                .Where(pro => 
                pro.Id == productId && pro.SellerId == User.GetUserId() 
                && pro.Category == Categories.ComputersAndLaptops && pro.SubCategory == laptopDto.Type)
                .Include(pro => pro.Laptop).FirstOrDefaultAsync();
            if (product == null)
                return NotFound();


            _mapper.Map(laptopDto, product);
            _mapper.Map(laptopDto, product.Laptop);

            await _sellerRepository.UpdateChangesToShopping(product);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPut("edit-product-vehicles/{productId}")]
        public async Task<ActionResult> EditVehicle(int productId, CreateVehicleDto createVehicleDto)
        {
            var product = await _context.Products
                .Where(pro => pro.Id == productId && pro.SellerId == User.GetUserId() && pro.Category == Categories.Vehicles)
                .Include(pro => pro.Vehicle).FirstOrDefaultAsync();
            if (product == null)
                return NotFound();


            _mapper.Map(createVehicleDto, product);
            _mapper.Map(createVehicleDto, product.Vehicle);

            product.SubCategory = createVehicleDto.Type;

            await _sellerRepository.UpdateChangesToShopping(product);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPut("edit-product-pets/{productId}")]
        public async Task<ActionResult> EditPet(int productId, CreatePetDto createPetDto)
        {
            var product = await _context.Products
                .Where(pro => pro.Id == productId && pro.SellerId == User.GetUserId() && pro.Category == Categories.Pets)
                .Include(pro => pro.Pet).FirstOrDefaultAsync();
            if (product == null)
                return NotFound();

            _mapper.Map(createPetDto, product);
            _mapper.Map(createPetDto, product.Pet);
            product.SubCategory = createPetDto.PetName;

            await _sellerRepository.UpdateChangesToShopping(product);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProductDto>(product));
        }
      
        [HttpPut("edit-product-mobile&tablets/{productId}")]
        public async Task<ActionResult> EditMobile(int productId, CreateMobilesDto mobileDto)
        {
            var product = await _context.Products
                .Where(pro => pro.Id == productId && pro.SellerId == User.GetUserId() && pro.Category == Categories.MobilesAndTablets)
                .Include(pro => pro.Mobile).FirstOrDefaultAsync();
            if (product == null)
                return NotFound();

            _mapper.Map(mobileDto, product);
            _mapper.Map(mobileDto, product.Mobile);
            product.SubCategory = mobileDto.Type;

            await _sellerRepository.UpdateChangesToShopping(product);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPut("edit-product-accessories/{productId}")]
        public async Task<ActionResult> EditAccessories(int productId, CreateAccessoriesDto accessoriesDto)
        {
            var product = await _context.Products
                .Where(pro => pro.Id == productId && pro.SellerId == User.GetUserId() && pro.Category == accessoriesDto.Category)
                .Include(pro => pro.Accessories).FirstOrDefaultAsync();
            if (product == null)
                return NotFound();

            _mapper.Map(accessoriesDto, product);
            _mapper.Map(accessoriesDto, product.Accessories);

            await _sellerRepository.UpdateChangesToShopping(product);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPut("edit-product-monitors/{productId}")]
        public async Task<ActionResult> EditMonitor(int productId, CreateMonitorDto monitorDto)
        {
            var product = await _context.Products
                .Where(pro => pro.Id == productId && pro.SellerId == User.GetUserId() && pro.Category == Categories.ComputersAndLaptops)
                .Include(pro => pro.MonitorProduct).FirstOrDefaultAsync();
            if (product == null)
                return NotFound();

            _mapper.Map(monitorDto, product);
            _mapper.Map(monitorDto, product.MonitorProduct);

            await _sellerRepository.UpdateChangesToShopping(product);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProductDto>(product));
        }




        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("my-profile")]
        public async Task<ActionResult> GetMyProfile()
        {
            var profile = await _context.Users.Where(u => u.Id == User.GetUserId())
                .ProjectTo<SellerProfileDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return Ok(profile);
        }

        [HttpGet("get-products-views")]
        public async Task<ActionResult> GetProductsViews([FromQuery] PaginationParams paginationParams)
        {
            ///gey my products with their views indication

            var products = _context.Products
                .Where(p => p.SellerId == User.GetUserId())
                .ProjectTo<ProductViewStatics>(_mapper.ConfigurationProvider)
                .OrderBy(p => p.State)
                .ThenByDescending(p => p.ProductViews);

            var result = await PagedList<ProductViewStatics>
            .CreateAsync(products, paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }



        [HttpGet("get-deleted-products")]
        public async Task<ActionResult> GetDeleteds([FromQuery] PaginationParams paginationParams)
        {
            var products = _context.Products
                .Where(pro => pro.SellerId == User.GetUserId() && pro.State == "deleted");

            var result = await PagedList<ProductCardDto>
            .CreateAsync(products.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }

        [HttpGet("get-banned-products")]
        public async Task<ActionResult> GetBannded([FromQuery] PaginationParams paginationParams)
        {
            var products = _context.Products
                .Where(pro => pro.SellerId == User.GetUserId() && pro.State == States.banned);

            var result = await PagedList<ProductCardDto>
            .CreateAsync(products.ProjectTo<ProductCardDto>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }

        [HttpGet("get-my-product/{id}")]
        public async Task<ActionResult> GetMyProduct(int id)
        {
            var products = _context.Products
                .Where(pro => pro.Id == id && pro.SellerId == User.GetUserId()); ///whatever state is

            var result = await products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPost("republish-product/{id}")]
        public async Task<ActionResult> Republish(int id)
        {
            var product = await _sellerRepository.GetProductAsync(id, User.GetUserId());

            if (product == null)
                return NotFound();

            if (product.State != "deleted")
                return BadRequest("this product not deleted!");

            product.State = "active";

            return Ok();
        }

        [HttpDelete("delete-product/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _sellerRepository.GetProductAsync(id, User.GetUserId());
            if (product == null)
            {
                return NotFound();
            }
            product.State = "deleted";

            if (await _productRepository.SaveAllAsync())
            {
                return Ok();
            }

            return BadRequest("Something went wrong during delete product");
        }

        [HttpDelete("delete-photo-from-product/{productId}/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int productId, int photoId)
        {
            var product = await _sellerRepository.GetProductAsync(productId, User.GetUserId());

            if (product == null)
                return NotFound();

            var photo = product.ProductPhotos.FirstOrDefault(p => p.Id == photoId);
            if (photo == null)
                return NotFound();


            ///remove from cloud

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);

                if (result.Error != null)
                {
                    return BadRequest(result.Error.Message);
                }
            }

            ///remove from database productPhotos

            product.ProductPhotos.Remove(photo);

            if (await _productRepository.SaveAllAsync())
            {
                return Ok();
            }

            return BadRequest("Something went wrong can't delete product photo");
        }

        [HttpPost("add-photo-to-product/{productId}")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(int productId, [Required] IFormFile file)
        {
            var product = await _sellerRepository.GetProductAsync(productId, User.GetUserId());
            if (product == null)
            {
                return NotFound();
            }
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            var photo = new ProductPhoto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                ProductId = productId
            };

            product.ProductPhotos.Add(photo);

            if (await _productRepository.SaveAllAsync())
            {
                return _mapper.Map<PhotoDto>(photo);
            }
            return BadRequest("Can't Add Photo");
        }

        [HttpGet("get-my-product")]
        public async Task<ActionResult> GetMyProducts([FromQuery]PaginationParams paginationParams)
        {
            var myproducts = await _sellerRepository.GetProductsAsync(User.GetUserId(), paginationParams);

            Response.AddPaginationHeader(new PaginationHeader(myproducts.CurrentPage, myproducts.PageSize, myproducts.TotalCount, myproducts.TotalPages));

            return Ok(myproducts);
        }

        [HttpGet("get-product-messages/{id}")]
        public async Task<ActionResult> GetProductMessages(int id)
        {
            return Ok(await _sellerRepository.GetProductMessages(id, User.GetUsername()));
        }
        [HttpGet("get-payments")]
        public async Task<ActionResult> GetPayments([FromQuery]PaginationParams paginationParams)
        {
            return Ok(await _sellerRepository.GetPayments(User.GetUserId(), paginationParams));
        }


        [HttpPut("edit-account")]
        public async Task<ActionResult> EditAccount(UpdateSellerDto updateSellerDto)
        {
            var seller = await _userRepository.GetUserByIdAsync(User.GetUserId());

            ///make validation for phone number if it's changed

            updateSellerDto.PhoneNumber = GenericMethod.GetPhoneNumberFormat(updateSellerDto.PhoneNumber);

            if (seller.PhoneNumber != updateSellerDto.PhoneNumber)
            {

                ///check if phone is taken by other user
                if (await PhoneNumberIsTaken(updateSellerDto.PhoneNumber))
                {
                    return BadRequest("Phone number is taken for different user");
                }

                ///check if phone number is real or fake
                if (!GenericMethod.CheckMobileNumber(updateSellerDto.PhoneNumber))
                {
                    return BadRequest("Invalid phone number");
                }
            }

            _mapper.Map(updateSellerDto, seller);

            await _userRepository.SaveAllAsync();

            return Ok();
        }


        [HttpPut("discount/{id}")]
        public async Task<ActionResult> Discount(int id,[FromQuery] int newDiscountRate)
        {
            var product = await _sellerRepository.GetProductAsync(id, User.GetUserId());
            if (product == null)
                return NotFound();

            if (newDiscountRate < 0 || newDiscountRate > 100)
                return BadRequest("discount rate between 0 and 100");

            product.DiscountRate = newDiscountRate;

            ///if this product exist in the cart change it's value into last update
            
            var cartItems = await _context.Carts
                .Where(cart => cart.ProductId == product.Id).ToListAsync();
            
            foreach (var cartItem in cartItems)
            {
                cartItem.Price = product.Price - product.Price * product.DiscountRate / 100.0;
                cartItem.Total = cartItem.Price * cartItem.Quantity;
            }

            return Ok(_mapper.Map<ProductDto>(product));
        }

        ///private
        private async Task<bool> PhoneNumberIsTaken(string phoneNumber)
        {
            return await _userManager.Users.AnyAsync(user => user.PhoneNumber == phoneNumber);
        }
    }
}
