using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceOrder.Persistence
{
    [Table("Pricing")]
    public class Pricing
    {
        [Key]
        public int Id { get; set; }
        public string ProductId { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
