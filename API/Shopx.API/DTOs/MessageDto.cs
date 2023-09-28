using Shopx.API.Entities;

namespace Shopx.API.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string SenderId { get; set; }
        public string SenderUsername { get; set; }
        public PhotoDto SenderBackgroundPhoto { get; set; }
        public string RecipenetId { get; set; }
        public string RecipenetUsername { get; set; }
        public PhotoDto RecipenetBackgroundPhoto { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
        public int UnreadCount { get; set; }
    }
}
