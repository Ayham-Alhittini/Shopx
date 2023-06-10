using System.ComponentModel.DataAnnotations;

namespace Shopx.API.Helper
{
    public class ResetPassword
    {
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

    }
}
