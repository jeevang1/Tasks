using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceOrder.Entities
{
    public class OrederInfo
    {
        public EcomUser User { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string CreditCardNo { get; set; }
    }
}
