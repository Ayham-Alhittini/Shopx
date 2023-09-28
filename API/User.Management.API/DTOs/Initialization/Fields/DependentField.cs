namespace Shopx.API.DTOs.Initialization.Fields
{
    public class DependentField: CommonFiled
    {
        public DependentField() { }
        public DependentField(string label, string type, List<DependentOption> options, string dependOn)
        {
            Options = options;
            Label = label;
            Type = type;
            DependsOn = dependOn;
        }
        public string DependsOn { get; set; }
        public List<DependentOption> Options { get; set; }
    }
}
