using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shopx.API.Data;
using Shopx.API.Data.Repository;
using Shopx.API.DTOs;
using Shopx.API.DTOs.Create_Products;
using Shopx.API.DTOs.Product_Specification;
using Shopx.API.Entities;
using Shopx.API.Entities.Product_Specification;
using Shopx.API.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;

namespace Shopx.API.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<AppUser, SellerDto>()
                .ForMember(d => d.isOnline, opt => opt.MapFrom(src => src.LastActive.GetState()));

            CreateMap<AppUser, SellerCardDto>();

            CreateMap<ShoppingCart, ProductCardDto>()
                .ForMember(d => d.ProductPhotos, opt => opt.MapFrom(src => src.Product.ProductPhotos));

            CreateMap<AppUser, SellerProfileDto>();

            CreateMap<AppUser, SellerRequestDto>();

            CreateMap<AppUser, CustomerDto>()
                .ForMember(d => d.isOnline, opt => opt.MapFrom(src => src.LastActive.GetState()));

            CreateMap<AppUser, UserDto>()
                .ForMember(d => d.BackgroundPhotoUrl, opt => opt.MapFrom(src => src.BackgroundPhoto.Url));

            CreateMap<UpdateSellerDto, AppUser>();

            CreateMap<BackgroundPhoto, PhotoDto>();
            CreateMap<ProductPhoto, PhotoDto>();

            CreateMap<Product, ProductViewStatics>();

            CreateMap<Product, ProductDto>()
                .ForMember(d => d.PriceAfterDiscount, opt => opt.MapFrom(src => src.Price - src.Price * src.DiscountRate / 100.0))
                .ForMember(d => d.Specification, opt => opt.MapFrom(src => GenericMethod.GetSpecification(src.Category, src.SubCategory)))
                .ForMember(d => d.link, opt => opt.MapFrom(src => GenericMethod.GetProductLink(src.Category, src.SubCategory)))
                .ForMember(d => d.ReportCount, opt => opt.MapFrom(src => src.ReportProducts.Where(r => r.WatchDate == null).ToList().Count));

            CreateMap<Product, ProductCardDto>()
                .ForMember(d => d.PriceAfterDiscount, opt => opt.MapFrom(src => src.Price - src.Price * src.DiscountRate / 100.0))
                .ForMember(d => d.link, opt => opt.MapFrom(src => GenericMethod.GetProductLink(src.Category, src.SubCategory)))
                .ForMember(d => d.ReportCount, opt => opt.MapFrom(src => src.ReportProducts.Where(r => r.WatchDate == null).ToList().Count));

            ///make it without report count make report count only for admin

            CreateMap<Sales, SalesDto>();
            CreateMap<Sales, ProductCardDto>();

            CreateMap<Follows, FollowDto>();


            CreateMap<ShoppingCart, CartDto>();

            CreateMap<Message, MessageDto>()
                .ForMember(d => d.SenderBackgroundPhoto, opt => opt
                .MapFrom(src => src.Sender.BackgroundPhoto == null ? null : src.Sender.BackgroundPhoto))

                .ForMember(d => d.RecipenetBackgroundPhoto, opt => opt
                .MapFrom(src => src.Recipenet.BackgroundPhoto == null ? null : src.Recipenet.BackgroundPhoto));


            CreateMap<AddressDto, Address>();
            CreateMap<Address, AddressDto>();

            CreateMap<ShoppingCart, Sales>();

            CreateMap<Order, OrderDto>();

            CreateMap<Sales, PaymentDto>();


            CreateMap<Notification, NotificationDto>();

            CreateMap<CreateProductReviewDto, ProductReview>();

            CreateMap<ProductReview, ReviewDto>()
                .ForMember(d => d.VoteRating, opt => opt.MapFrom(src => src.ProductReviewVotes.Select(v => v.VoteValue).Sum()));


            CreateMap<ShopReview, ReviewDto>()
                .ForMember(d => d.VoteRating, opt => opt.MapFrom(src => src.ShopReviewVotes.Select(v => v.VoteValue).Sum()));


            CreateMap<CreateShopReviewDto, ShopReview>();

            CreateMap<ReportProduct, ReportDto>()
                .ForMember(d => d.KnownAs, opt => opt.MapFrom(src => src.Customer.KnownAs))
                .ForMember(d => d.BackgroundUrl, opt => opt.MapFrom(src => src.Customer.BackgroundPhoto.Url));


            ///Laptop and computer
            CreateMap<CreateLaptopDto, LaptopAndComputer>();
            CreateMap<CreateLaptopDto, Product>();
            CreateMap<LaptopAndComputer, LaptopDto>();

            ///Vehicle
            CreateMap<CreateVehicleDto, Vehicle>();
            CreateMap<CreateVehicleDto, Product>();
            CreateMap<Vehicle, VehicleDto>();

            ///Pet
            CreateMap<CreatePetDto, Pet>();
            CreateMap<CreatePetDto, Product>();
            CreateMap<Pet, PetDto>();

            ///Mobile
            CreateMap<CreateMobilesDto, Mobile>();
            CreateMap<CreateMobilesDto, Product>();
            CreateMap<Mobile, MobileDto>();

            ///Accessories
            CreateMap<CreateAccessoriesDto, Accessories>()
                .ForMember(d => d.Type, opt => opt.MapFrom(src => src.AccessoriesType));
            CreateMap<CreateAccessoriesDto, Product>();
            CreateMap<Accessories, AccessoriesDto>()
                .ForMember(d => d.AccessoriesType, opt => opt.MapFrom(src => src.Type));

            ///Monitor
            CreateMap<CreateMonitorDto, MonitorProduct>();
            CreateMap<CreateMonitorDto, Product>();
            CreateMap<MonitorProduct, MonitorProductDto>();
        }
    }
}
