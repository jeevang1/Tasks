using ECommerceOrder.Entities;
using System.Threading.Tasks;

namespace ECommerceOrder.Order
{
    public interface IOrderProcessingHelper
    {
        Task<OrderProcessingResult> ProcessOrder(OrederInfo orderDetails);
    }
}
