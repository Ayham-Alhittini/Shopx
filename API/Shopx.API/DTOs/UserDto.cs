using Shopx.API.Entities;

namespace Shopx.API.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string KnownAs { get; set; }
        public string AccountState { get; set; }
        public string AccountType { get; set; }
        public string BackgroundPhotoUrl { get; set; }
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public DateTime Created { get; set; }
    }
}
