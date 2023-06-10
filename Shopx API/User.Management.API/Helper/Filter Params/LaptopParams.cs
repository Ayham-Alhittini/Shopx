namespace Shopx.API.Helper.Filter_Params
{
    public class LaptopParams: CommonParams
    {
        public List<string> Brand { get; set; } = new();
        public List<string> OperatingSystem { get; set; } = new();
        public List<string> ScreenSize { get; set; } = new();
        public List<int> Ram { get; set; } = new();
        public string Type { get; set; }


    }
}
