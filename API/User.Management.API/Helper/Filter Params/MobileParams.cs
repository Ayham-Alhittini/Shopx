namespace Shopx.API.Helper.Filter_Params
{
    public class MobileParams: CommonParams
    {
        public string Type { get; set; }
        public List<string> Brand { get; set; } = new();
        public List<string> Model { get; set; } = new();
        public List<string> StorageSize { get; set; } = new();
        public List<string> Color { get; set; } = new();
        public List<string> ScreenSize { get; set; } = new();
    }
}
