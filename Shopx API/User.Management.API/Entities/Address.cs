using System.ComponentModel.DataAnnotations.Schema;

namespace Shopx.API.Entities
{
    [Table("Adresses")]
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public int PostCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
    }
}
