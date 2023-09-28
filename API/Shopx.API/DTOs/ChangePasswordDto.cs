using System.ComponentModel.DataAnnotations;

namespace Shopx.API.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }


        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
    }
}
