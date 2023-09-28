namespace Shopx.API.DTOs.Initialization.Options
{
    public class ComputerAndLaptopOptions
    {
        public List<string> Brand { get; set; }
        public List<DependentOption> OperatingSystem { get; set; }
        public List<string> ScreenSize { get; set; }
    }
}
