using Microsoft.AspNetCore.Identity;

namespace Shopx.API.Entities
{
    public class AppUser: IdentityUser
    {
        ///common attribute 
        public string KnownAs { get; set; }
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public BackgroundPhoto BackgroundPhoto { get; set; }
        public string AccountState { get; set; }
        public string AccountType { get; set; }
        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesRecived { get; set; }
        public List<Order> Orders { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<ProductReviewVote> ProductReviewVotes { get; set; }
        public List<ShopReviewVote> ShopReviewVotes { get; set; }   

        ///Seller attribute
        public string ShopDescription { get; set; }
        public string ShopCity { get; set; }
        public string TaxNumber { get; set; }
        public string FullName { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }

        public List<Product> Products { get; set; } = new();
        public List<Follows> Follower { get; set; }
        public List<ShopReview> Reviews { get; set; }


        ///Customer attribute
        public List<Sales> BoughtProduct { get; set; }
        public List<ShoppingCart> Carts { get; set; }
        public List<Follows> Following { get; set; }
        public List<WishList> Wishlists { get; set; }
        public List<ProductReview> ProductReviews { get; set; }
        public List<ShopReview> ShopReviews { get; set; }
        public List<BrowseHistory> BrowseHistories { get; set; }
        public List<ReportProduct> ReportProducts { get; set; }
    }
}
