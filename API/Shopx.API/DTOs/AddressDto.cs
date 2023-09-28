using System.ComponentModel.DataAnnotations;

namespace Shopx.API.DTOs
{
    public class AddressDto
    {
        [Required]
        public string City { get; set; }
        [Required]
        public int PostCode { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
    }
}
