namespace ECommerceOrder.Payment
{
    public class CreditCardHelper : ICreditCardHelper
    {
        dynamic _creditCardService;
        public CreditCardHelper()
        {
            //Implementation of this class largely depends on third party service which may be a dll or a web service.
            //In case of service We need to get the url, secrets and other keys from configuration and initiate a client which is cached at class level 
            _creditCardService = "Some service client obtained above";
        }
        public bool ValidateCreditCard(string creditCardNo)
        {
            return _creditCardService.ValidateCreditCard(creditCardNo);
        }
    }
}
