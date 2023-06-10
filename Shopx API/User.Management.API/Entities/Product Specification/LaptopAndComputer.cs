using System.ComponentModel.DataAnnotations.Schema;

namespace Shopx.API.Entities.Product_Specification
{
    [Table("Laptops_And_Computers")]
    public class LaptopAndComputer
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string OperatingSystem { get; set; }
        public string ScreenSize { get; set; }
        public int Ram { get; set; }
        public string Type { get; set; }
    }
}
