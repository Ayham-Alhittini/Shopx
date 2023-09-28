namespace Shopx.API.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public bool Read { get; set; }
        public DateTime SendDate { get; set; } = DateTime.UtcNow;
    }
}
