using System.ComponentModel.DataAnnotations;

namespace Shopx.API.DTOs
{
    public class RegisterCustomerDto
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Phone]
        [Required(ErrorMessage = "Phone Number is required")]
        [MinLength(12)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Known as required")]
        public string KnownAs { get; set; }
    }
}
