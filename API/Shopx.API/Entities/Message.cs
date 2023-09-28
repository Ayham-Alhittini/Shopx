namespace Shopx.API.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string SenderId { get; set; }
        public string SenderUsername { get; set; }
        public AppUser Sender { get; set; }
        public string RecipenetId { get; set; }
        public string RecipenetUsername { get; set; }
        public AppUser Recipenet { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
        public bool SenderDeleted { get; set; }
        public bool RecipenetDeleted { get; set; }
        public bool LastMessage { get; set; } = true;
        public int UnreadCount { get; set; }
    }
}
