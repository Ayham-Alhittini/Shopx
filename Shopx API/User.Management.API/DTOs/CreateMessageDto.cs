namespace Shopx.API.DTOs
{
    public class CreateMessageDto
    {
        public string RecipenetUsername { get; set; }
        public string Content { get; set; }
        public int ProductId { get; set; }
    }
}
