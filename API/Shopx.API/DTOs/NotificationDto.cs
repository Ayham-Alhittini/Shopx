using Shopx.API.Entities;

namespace Shopx.API.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Read { get; set; }
    }
}
