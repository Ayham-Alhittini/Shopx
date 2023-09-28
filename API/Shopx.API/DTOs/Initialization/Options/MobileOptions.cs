namespace Shopx.API.DTOs.Initialization.Options
{
    public class MobileOptions
    {
        public List<string> Brand { get; set; }
        public List<DependentOption> Model { get; set; }
        public List<string> StorageSize { get; set; }
        public List<string> Color { get; set; }
        public List<string> ScreenSize { get; set; }
    }
}
