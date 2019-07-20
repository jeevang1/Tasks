using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceOrder.Entities
{
    public class EcomUser
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string DeliveryAddress { get; set; }

    }
}
