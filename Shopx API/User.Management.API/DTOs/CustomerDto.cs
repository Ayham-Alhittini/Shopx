namespace Shopx.API.DTOs
{
    public class CustomerDto
    {
        public string Id { get; set; }
        public string KnownAs { get; set; }
        public bool isOnline { get; set; }
        public string  Email { get; set; }
        public DateTime Created { get; set; }
        public PhotoDto BackgroundPhoto { get; set; }
    }
}
