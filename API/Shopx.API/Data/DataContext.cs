using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopx.API.Entities;
using Shopx.API.Entities.Product_Specification;
using System.Reflection.Emit;

namespace Shopx.API.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<Product> Products { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Follows> Follows { get; set; }
        public DbSet<BackgroundPhoto> Backgrounds { get; set; }
        public DbSet<ShoppingCart> Carts { get; set; }
        public DbSet<LaptopAndComputer> Laptops { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Mobile> MobilesAndTablets { get; set; }
        public DbSet<Accessories> Accessories { get; set; }
        public DbSet<MonitorProduct> MonitorProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<WishList> Wishlists { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ShopReview> ShopReviews { get; set; }
        public DbSet<BrowseHistory> BrowseHistories { get; set; }
        public DbSet<ReportProduct> Reports { get; set; }
        public DbSet<ProductReviewVote> ProductReviewVotes { get; set; }
        public DbSet<ShopReviewVote> ShopReviewVotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Follows>()
                .HasKey(l => new { l.SourceUserId, l.TargetUserId });

            builder.Entity<Follows>()
                .HasOne(l => l.SourceUser)
                .WithMany(l => l.Following)
                .HasForeignKey(l => l.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Follows>()
                .HasOne(l => l.TargetUser)
                .WithMany(l => l.Follower)
                .HasForeignKey(l => l.TargetUserId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<ShoppingCart>()
                .HasKey(s => new {s.CustomerId, s.ProductId});
            
            builder.Entity<WishList>()
                .HasKey(s => new {s.CustomerId, s.ProductId});
            
            
            builder.Entity<BrowseHistory>()
                .HasKey(s => new { s.CustomerId, s.ProductId });
            
            
            builder.Entity<ProductReviewVote>()
                .HasKey(s => new { s.UserId, s.ProductReviewId});

             builder.Entity<ShopReviewVote>()
                .HasKey(s => new { s.UserId, s.ShopReviewId});

            builder.Entity<ShopReview>()
                .HasOne(r => r.Customer)
                .WithMany(r => r.ShopReviews)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ShopReview>()
                .HasOne(r => r.Seller)
                .WithMany(r => r.Reviews)
                .OnDelete(DeleteBehavior.NoAction);

            
            builder.Entity<ShopReviewVote>()
                .HasOne(v => v.User)
                .WithMany(v => v.ShopReviewVotes)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ShopReviewVote>()
                .HasOne(v => v.ShopReview)
                .WithMany(v => v.ShopReviewVotes)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.Recipenet)
                .WithMany(m => m.MessagesRecived)
                .OnDelete(DeleteBehavior.Restrict);

            

        }
    }
}
