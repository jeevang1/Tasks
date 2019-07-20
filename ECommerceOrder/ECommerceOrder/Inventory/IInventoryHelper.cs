using ECommerceOrder.Persistence;
using System.Linq;

namespace ECommerceOrder.Inventory
{
    public interface IInventoryHelper
    {
        bool CheckInventory(string productId, int quantity);
        decimal GetProcuctPrice(string productId);
    }

    public class InventoryHelper : IInventoryHelper
    {
        IUnitOfWork _unitOfWork;
        public InventoryHelper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool CheckInventory(string productId, int quantity)
        {
            return _unitOfWork.Inventory.Any(x => x.ProductId == productId && x.CurrentStock >= quantity);
        }
        public decimal GetProcuctPrice(string productId)
        {
            var result = decimal.MaxValue;
            var detailsFromDb = _unitOfWork.Pricing.FirstOrDefault(x => x.ProductId == productId);
            if (detailsFromDb != default(Pricing))
            {
                result = detailsFromDb.CurrentPrice;
            }
            return result;
        }
    }
}
