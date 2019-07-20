namespace ECommerceOrder.Payment
{
    public interface ICreditCardHelper
    {
        bool ValidateCreditCard(string creditCardNo);
    }
}
