using ECommerceOrder.Inventory;
using ECommerceOrder.Mails;
using ECommerceOrder.Payment;
using ECommerceOrder.Persistence;

namespace ECommerceOrder
{
    public interface IHelperFactory
    {
        IInventoryHelper GetInventoryHelper();
        IPaymentServiceHelper GetPaymentHelper();
        ICreditCardHelper GetCreditCardHelper();
        IMailsHelper GetMailHelper();
    }

    public class HelperFactory : IHelperFactory
    {
        IUnitOfWork _unitOfWork;
        IUnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null)
                {
                    //Instantiate here the production calss of Unit of work 
                    //which may be wrapper around the entity framework
                }
                return _unitOfWork;
            }
        }
        ICreditCardHelper _creditCardHelper;
        public ICreditCardHelper GetCreditCardHelper()
        {
            if (_creditCardHelper == null)
            {
                _creditCardHelper = new CreditCardHelper();
            }
            return _creditCardHelper;
        }
        IInventoryHelper _inventoryHelper;
        public IInventoryHelper GetInventoryHelper()
        {
            if (_inventoryHelper == null)
            {
                _inventoryHelper = new InventoryHelper(UnitOfWork);
            }
            return _inventoryHelper;
        }
        IMailsHelper _mailsHelper;
        public IMailsHelper GetMailHelper()
        {
            if (_mailsHelper == null)
            {
                _mailsHelper = new MailsHelper();
            }
            return _mailsHelper;
        }
        IPaymentServiceHelper _paymentServiceHelper;
        public IPaymentServiceHelper GetPaymentHelper()
        {
            if (_paymentServiceHelper == null)
            {
                _paymentServiceHelper = new PaymentServiceHelper();
            }
            return _paymentServiceHelper;
        }
    }
}
