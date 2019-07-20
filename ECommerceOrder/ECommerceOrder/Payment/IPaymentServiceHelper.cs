namespace ECommerceOrder.Payment
{
    public interface IPaymentServiceHelper
    {
        bool ChargePayment(string creditCardNo, decimal amountToCharge);
        bool RevertPayment(string creditCardNo, decimal amountToRevert);
    }
}
