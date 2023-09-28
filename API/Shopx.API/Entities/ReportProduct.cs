namespace Shopx.API.Entities
{
    public class ReportProduct
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public AppUser Customer { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ReportReason { get; set; }
        public string ReportDetails { get; set; }
        public DateTime SendDate { get; set; } = DateTime.UtcNow;
        public DateTime? WatchDate { get; set; } = null;
    }
}
