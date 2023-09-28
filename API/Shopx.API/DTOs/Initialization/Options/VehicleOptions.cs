namespace Shopx.API.DTOs.Initialization.Options
{
    public class VehicleOptions
    {
        public List<string> CarMake { get; set; }
        public List<DependentOption> Model { get; set; }
        public List<string> Transmission { get; set; }
        public List<string> Fuel { get; set; }
        public List<string> Color { get; set; }
        public List<string> Condition { get; set; }
        public List<string> Kilometers { get; set; }
        public List<string> Paint { get; set; }
    }
}
