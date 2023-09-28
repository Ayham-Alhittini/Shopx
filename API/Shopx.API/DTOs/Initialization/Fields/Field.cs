namespace Shopx.API.DTOs.Initialization.Fields
{
    public class Field: CommonFiled
    {
        public Field() { }
        public Field(string label, string type)
        {
            Label = label;
            Type = type;
        }
        public Field(string label, string type, List<string>options)
        {
            Options = options;
            Label = label;
            Type = type;
        }
        public List<string> Options { get; set; }
    }
}
