using System.ComponentModel.DataAnnotations;

namespace Shopx.API.Helper
{
    public class ManageProductParams: PaginationParams
    {
        [Required]
        public string ProductState { get; set; } = "active";
        public string SellerName { get; set; }
    }
}
