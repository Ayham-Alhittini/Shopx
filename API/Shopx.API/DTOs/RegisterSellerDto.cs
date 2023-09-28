using Shopx.API.Data;
using System.ComponentModel.DataAnnotations;

namespace Shopx.API.DTOs
{
    public class RegisterSellerDto
    {
        ///private fileds
        private string _shopCity;


        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }


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

        [Required(ErrorMessage = "Tax Number is required")]
        public string TaxNumber { get; set; }

        [Required(ErrorMessage = "Shop city is required")]
        
        public string ShopCity
        {
            get { return _shopCity; }
            set 
            {
                if (!_options.Cities().Contains(value))
                    throw new Exception("City not exist");
                _shopCity = value;
            }
        }
        public string ShopDescription { get; set; } = "";

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Bank Name is required")]
        public string BankName { get; set; }
        
        [Required(ErrorMessage = "Bank Account Number is required")]
        public string BankAccountNumber { get; set; }

    }
}
