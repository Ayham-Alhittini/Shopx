namespace Shopx.API.Entities
{
    public class BrowseHistory
    {
        public string CustomerId { get; set; }
        public AppUser Customer { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime BrowseDate { get; set; } = DateTime.UtcNow;
    }
}
