namespace Shopx.API.Helper.Filter_Params
{
    public class MonitorParams: CommonParams
    {
        public List<string> Brand { get; set; } = new();
        public List<string> ScreenSize { get; set; } = new();
    }
}
