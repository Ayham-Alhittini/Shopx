namespace Shopx.API.Helper
{
    public class FollowParams: PaginationParams
    {
        public string Predicate { get; set; }
        public string UserId { get; set; }
    }
}
