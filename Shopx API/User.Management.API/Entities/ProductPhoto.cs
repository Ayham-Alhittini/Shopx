namespace Shopx.API.Entities
{
    public class ProductPhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public Product Product { get; set; }
    }
}
