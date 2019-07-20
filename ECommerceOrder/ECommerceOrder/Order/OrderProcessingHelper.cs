using ECommerceOrder.Entities;
using System.Threading.Tasks;

namespace ECommerceOrder.Order
{
    public class OrderProcessingHelper : IOrderProcessingHelper
    {
        IHelperFactory _helperFactory;
        public OrderProcessingHelper(IHelperFactory helperFactory)
        {
            _helperFactory = helperFactory;
        }
        public Task<OrderProcessingResult> ProcessOrder(OrederInfo orderDetails)
        {
            var result = new OrderProcessingResult();
            //check availability
            var isAvailable =_helperFactory.GetInventoryHelper().CheckInventory(orderDetails.ProductId, orderDetails.Quantity);
            //validate credit card
            if (isAvailable)
            {
                var isCreditCardValid = _helperFactory.GetCreditCardHelper().ValidateCreditCard(orderDetails.CreditCardNo);
                if (isCreditCardValid)
                {
                    decimal unitPrice = _helperFactory.GetInventoryHelper().GetProcuctPrice(orderDetails.ProductId);
                    decimal amountToCharge = unitPrice * orderDetails.Quantity;
                    //deduct payment
                    bool isPaymentSucceeded = _helperFactory.GetPaymentHelper().ChargePayment(orderDetails.CreditCardNo, amountToCharge);
                    if (isPaymentSucceeded)
                    {
                        //Send Email
                        var mailAttempt = 1;
                        var isMailSent = false;
                        while (! isMailSent && mailAttempt++ <= 3)
                        {
                            isMailSent = _helperFactory.GetMailHelper().SendOrderMailToOrderTeam(orderDetails);
                        }
                        if (!isMailSent)
                        {
                            //ideally should be retried to revert payment.
                            _helperFactory.GetPaymentHelper().RevertPayment(orderDetails.CreditCardNo, amountToCharge);
                            result.ErrorMessage.Add("Order_Failed", "Unable to Process Order, Please try again later");
                        }
                        else
                        {
                            result.Result = true;
                        }
                    }
                    else
                    {
                        result.ErrorMessage.Add("Payment_Failed", "Payment Failed on credit card");
                    }
                }
                else
                {
                    result.ErrorMessage.Add("Credit_Card_Not_Valid", "Credit card is not valid");
                }
            }
            else
            {
                result.ErrorMessage.Add("Err_Not_Enough_Inventory", "Required Inventory Not Available");
            }
            return Task.FromResult(result);
        }
    }
}
