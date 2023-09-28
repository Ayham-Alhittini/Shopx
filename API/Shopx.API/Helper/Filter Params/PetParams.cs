namespace Shopx.API.Helper.Filter_Params
{
    public class PetParams: CommonParams
    {
        public string PetName { get; set; }
        public List<string> PetType { get; set; } = new();
    }
}
