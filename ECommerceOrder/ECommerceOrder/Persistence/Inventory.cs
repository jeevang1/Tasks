using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceOrder.Persistence
{
    [Table("Inventory")]
    public class Inventory
    {
        [Key]
        public int Id { get; set; }
        public string ProductId { get; set; }
        public int CurrentStock { get; set; }
    }
}
