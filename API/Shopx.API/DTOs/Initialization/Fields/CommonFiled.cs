namespace Shopx.API.DTOs.Initialization.Fields
{
    public class CommonFiled
    {
        public string Label { get; set; }
        public string Value { get; set; } = "";
        public string Type { get; set; }
        public List<string> Rules { get; set; } = new List<string> { "required" };
    }
}
