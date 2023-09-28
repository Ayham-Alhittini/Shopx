using Shopx.API.Entities;

namespace Shopx.API.DTOs
{
    public class SellerRequestDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public string ShopCity { get; set; }
        public string ShopDescription { get; set; }
        public string TaxNumber { get; set; }
        public string FullName { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
    }
}
