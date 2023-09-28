using Shopx.API.Entities;

namespace Shopx.API.DTOs
{
    public class ReportDto
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string KnownAs { get; set; }
        public string BackgroundUrl { get; set; }
        public int ProductId { get; set; }
        public string ReportReason { get; set; }
        public string ReportDetails { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime? WatchDate { get; set; }
    }
}
