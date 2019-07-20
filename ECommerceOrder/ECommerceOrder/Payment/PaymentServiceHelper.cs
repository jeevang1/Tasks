namespace ECommerceOrder.Payment
{
    public class PaymentServiceHelper : IPaymentServiceHelper
    {
        dynamic _paymentService;
        public PaymentServiceHelper()
        {
            //Implementation of this class largely depends on third party service which may be a dll or a web service.
            //In case of service We need to get the url, secrets and other keys from configuration and initiate a client which is cached at class level 
            _paymentService = "Some service client obtained above";
        }
        public bool ChargePayment(string creditCardNo, decimal amountToCharge)
        {
            return _paymentService.ChargePayment(creditCardNo, amountToCharge);
        }

        public bool RevertPayment(string creditCardNo, decimal amountToRevert)
        {
            return _paymentService.RevertPayment(creditCardNo, amountToRevert);
        }
    }
}
