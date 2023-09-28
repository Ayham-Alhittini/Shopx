namespace Shopx.API.Entities
{
    public class Follows
    {
        public AppUser SourceUser { get; set; }
        public string SourceUserId { get; set; }
        public AppUser TargetUser { get; set; }
        public string TargetUserId { get; set; }
    }
}
