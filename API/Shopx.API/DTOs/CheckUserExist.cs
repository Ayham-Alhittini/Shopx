using System.ComponentModel.DataAnnotations;

namespace Shopx.API.DTOs
{
    public class CheckUserExist
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
