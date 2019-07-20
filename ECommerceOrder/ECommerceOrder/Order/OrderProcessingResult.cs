using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceOrder.Order
{
    public class OrderProcessingResult
    {
        public OrderProcessingResult()
        {
            Result = false;
            ErrorMessage = new Dictionary<string, string>();
        }
        public bool Result { get; set; }
        public Dictionary<string,string> ErrorMessage { get; set; }
    }
}
