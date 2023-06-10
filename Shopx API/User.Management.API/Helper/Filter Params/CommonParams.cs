using System.ComponentModel.DataAnnotations;

namespace Shopx.API.Helper.Filter_Params
{
    public class CommonParams:PaginationParams
    {
        [MaxLength(20)]
        public string SearchContent { get; set; }
        public double MinPrice { get; set; } = 0;
        public double MaxPrice { get; set; } = double.MaxValue;
    }
}
