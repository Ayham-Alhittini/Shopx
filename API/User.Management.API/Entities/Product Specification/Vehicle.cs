using System.ComponentModel.DataAnnotations.Schema;

namespace Shopx.API.Entities.Product_Specification
{
    [Table("Vehicles")]
    public class  Vehicle
    {
        public int Id { get; set; }
        public string CarMake { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public string Transmission { get; set; }
        public string Fuel { get; set; }
        public string Color { get; set; }
        public string Condition { get; set; }
        public string Kilometers { get; set; }
        public string Paint { get; set; }
    }
}
