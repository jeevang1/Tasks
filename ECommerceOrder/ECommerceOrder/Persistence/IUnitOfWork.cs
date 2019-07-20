using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceOrder.Persistence
{
    //All the collections here ideally should be DbSet in case of entity framework
    public interface IUnitOfWork
    {
        IEnumerable<Inventory> Inventory { get; }
        IEnumerable<Pricing> Pricing { get; }
    }
}
