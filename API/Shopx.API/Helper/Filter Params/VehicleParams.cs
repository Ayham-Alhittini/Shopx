namespace Shopx.API.Helper.Filter_Params
{
    public class VehicleParams: CommonParams
    {
        public List<string> CarMake { get; set; } = new();
        public List<string> Model { get; set; } = new();
        public int YearFrom { get; set; } = 1970;
        public int YearTo { get; set; } = DateTime.Now.Year + 1;
        public List<string> Type { get; set; } = new();
        public List<string> Transmission { get; set; } = new();
        public List<string> Fuel { get; set; } = new();
        public List<string> Color { get; set; } = new();
        public List<string> Condition { get; set; } = new();
        public List<string> Kilometers { get; set; } = new();
        public List<string> Paint { get; set; } = new();
    }
}
