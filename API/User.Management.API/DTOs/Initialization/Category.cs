using Shopx.API.DTOs.Initialization;

namespace Shopx.API.DTOs.Initialization
{
    public class Category
    {
        public string label { get; set; }
        public string link { get; set; }
        public string logo { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
}
//public class Category
//{
//    public string label { get; set; }
//    public string link { get; set; }
//    public string logo { get; set; }
//    public List<SubCategory> SubCategories { get; set; }
//}