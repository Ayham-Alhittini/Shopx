namespace Shopx.API.Helper.Filter_Params
{
    public class AccessoriesParams: CommonParams
    {
        public string Categoriy { get; set; }
        public List<string> Type { get; set; } = new();
    }
}
