using ECommerceOrder.Entities;

namespace ECommerceOrder.Mails
{
    public interface IMailsHelper
    {
        bool SendOrderMailToOrderTeam(OrederInfo orderDetails);
    }
}
