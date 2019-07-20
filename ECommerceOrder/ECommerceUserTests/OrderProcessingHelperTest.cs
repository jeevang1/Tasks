using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ECommerceOrder;
using ECommerceOrder.Mails;
using ECommerceOrder.Payment;
using ECommerceOrder.Inventory;
using ECommerceOrder.Order;
using ECommerceOrder.Entities;

namespace ECommerceUserTests
{
    [TestClass]
    public class OrderProcessingHelperTest
    {
        Mock<IHelperFactory> _helperFactory;
        Mock<IMailsHelper> _mailsHelper;
        Mock<ICreditCardHelper> _creditCardHelper;
        Mock<IPaymentServiceHelper> _paymentServiceHelper;
        Mock<IInventoryHelper> _inventoryHelper;
        OrderProcessingHelper _orderProcessor;

        [TestInitialize]
        public void Initialize()
        {
            _helperFactory = new Mock<IHelperFactory>();

            _mailsHelper = new Mock<IMailsHelper>();
            _helperFactory.Setup(x => x.GetMailHelper()).Returns(_mailsHelper.Object);

            _creditCardHelper = new Mock<ICreditCardHelper>();
            _helperFactory.Setup(x => x.GetCreditCardHelper()).Returns(_creditCardHelper.Object);

            _paymentServiceHelper = new Mock<IPaymentServiceHelper>();
            _helperFactory.Setup(x => x.GetPaymentHelper()).Returns(_paymentServiceHelper.Object);

            _inventoryHelper = new Mock<IInventoryHelper>();
            _helperFactory.Setup(x => x.GetInventoryHelper()).Returns(_inventoryHelper.Object);
            
            _orderProcessor = new OrderProcessingHelper(_helperFactory.Object);
        }
        [TestMethod]
        public void If_Inventory_Not_Available_ProcessOrder_Returns_Error_Message()
        {
            var ErrorMessageKey = "Err_Not_Enough_Inventory";
            _inventoryHelper.Setup(x => x.CheckInventory(It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            var result = _orderProcessor.ProcessOrder(new OrederInfo()).Result;
            Assert.IsFalse(result.Result);
            Assert.IsTrue(result.ErrorMessage.ContainsKey(ErrorMessageKey));
            Assert.AreEqual(result.ErrorMessage[ErrorMessageKey], "Required Inventory Not Available");
        }
        [TestMethod]
        public void If_Credit_Card_Not_Valid_ProcessOrder_Returns_Error_Message()
        {
            var ErrorMessageKey = "Credit_Card_Not_Valid";
            _inventoryHelper.Setup(x => x.CheckInventory(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            _creditCardHelper.Setup(x => x.ValidateCreditCard(It.IsAny<string>())).Returns(false);
            var result = _orderProcessor.ProcessOrder(new OrederInfo()).Result;
            Assert.IsFalse(result.Result);
            Assert.IsTrue(result.ErrorMessage.ContainsKey(ErrorMessageKey));
            Assert.AreEqual(result.ErrorMessage[ErrorMessageKey], "Credit card is not valid");
        }

        [TestMethod]
        public void If_Payment_Failed_ProcessOrder_Returns_Error_Message()
        {
            var ErrorMessageKey = "Payment_Failed";
            _inventoryHelper.Setup(x => x.CheckInventory(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            _creditCardHelper.Setup(x => x.ValidateCreditCard(It.IsAny<string>())).Returns(true);
            _paymentServiceHelper.Setup(x => x.ChargePayment(It.IsAny<string>(), It.IsAny<decimal>())).Returns(false);
            var result = _orderProcessor.ProcessOrder(new OrederInfo()).Result;
            Assert.IsFalse(result.Result);
            Assert.IsTrue(result.ErrorMessage.ContainsKey(ErrorMessageKey));
            Assert.AreEqual(result.ErrorMessage[ErrorMessageKey], "Payment Failed on credit card");
        }

        [TestMethod]
        public void Send_Order_Mail_Is_Tried_Thrice_And_Payment_IS_Reverted_With_Error_Message()
        {
            var ErrorMessageKey = "Order_Failed";
            var mailSendingAttempt = 0;
            var orderDetails = new OrederInfo();
            var expectedCreditCardNo = "Some Credit Card No";
            var unitProductPrice = 100;
            
            orderDetails.CreditCardNo = expectedCreditCardNo;
            orderDetails.Quantity = 5;
            decimal expectedAmountToRevert = 500;

            string passedCardNoToRevert = string.Empty;
            decimal passedAmountToRevert = 0;

            _inventoryHelper.Setup(x => x.CheckInventory(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            _inventoryHelper.Setup(x => x.GetProcuctPrice(It.IsAny<string>())).Returns(unitProductPrice);

            _creditCardHelper.Setup(x => x.ValidateCreditCard(It.IsAny<string>())).Returns(true);
            _paymentServiceHelper.Setup(x => x.ChargePayment(It.IsAny<string>(), It.IsAny<decimal>())).Returns(true);
            _paymentServiceHelper.Setup(x => x.RevertPayment(It.IsAny<string>(), It.IsAny<decimal>())).Callback<string, decimal>((x, y) => { passedCardNoToRevert = x; passedAmountToRevert = y; });
            _mailsHelper.Setup(x => x.SendOrderMailToOrderTeam(It.IsAny<OrederInfo>())).Callback(()=> mailSendingAttempt++).Returns(false);

            var result = _orderProcessor.ProcessOrder(orderDetails).Result;
            Assert.AreEqual(mailSendingAttempt, 3);
            Assert.AreEqual(expectedCreditCardNo, passedCardNoToRevert);
            Assert.AreEqual(expectedAmountToRevert, passedAmountToRevert);
            Assert.IsFalse(result.Result);
            Assert.IsTrue(result.ErrorMessage.ContainsKey(ErrorMessageKey));
            Assert.AreEqual(result.ErrorMessage[ErrorMessageKey], "Unable to Process Order, Please try again later");
        }

        [TestMethod]
        public void When_Mail_Is_Sent_ProcessOrder_Returns_True_And_No_Error_Message()
        {
            var orderDetails = new OrederInfo();

            _inventoryHelper.Setup(x => x.CheckInventory(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            
            _creditCardHelper.Setup(x => x.ValidateCreditCard(It.IsAny<string>())).Returns(true);
            _paymentServiceHelper.Setup(x => x.ChargePayment(It.IsAny<string>(), It.IsAny<decimal>())).Returns(true);
            _paymentServiceHelper.Setup(x => x.RevertPayment(It.IsAny<string>(), It.IsAny<decimal>()));
            _mailsHelper.Setup(x => x.SendOrderMailToOrderTeam(It.IsAny<OrederInfo>())).Returns(true);

            var result = _orderProcessor.ProcessOrder(orderDetails).Result;
            Assert.IsTrue(result.Result);
            Assert.AreEqual(0 , result.ErrorMessage.Count);
        }
    }
}
