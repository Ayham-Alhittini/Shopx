namespace Shopx.API.DTOs
{
    public class SellerCardDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string KnownAs { get; set; }
        public PhotoDto BackgroundPhoto { get; set; }
        public string ShopCity { get; set; }
        public double ShopRate { get; set; }
    }
}
