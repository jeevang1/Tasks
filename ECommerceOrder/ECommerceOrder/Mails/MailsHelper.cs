using System;
using ECommerceOrder.Entities;
using System.Net.Mail;
using System.Configuration;
using System.Text;

namespace ECommerceOrder.Mails
{
    public class MailsHelper : IMailsHelper
    {
        MailAddress _from;
        string _orderTeamEmail;
        SmtpClient _mailClient; 
        public MailsHelper()
        {
            _orderTeamEmail = ConfigurationManager.AppSettings["OrderTeamEmailId"];
            var smtpDetails = ConfigurationManager.AppSettings["SmtpDetails"];
            _mailClient = new SmtpClient(smtpDetails);
            _from = new MailAddress(ConfigurationManager.AppSettings["FromEmailId"]);
        }
        public bool SendOrderMailToOrderTeam(OrederInfo orderDetails)
        {
            var result = false;
            MailAddress to = new MailAddress(ConfigurationManager.AppSettings["OrderTeamEmailId"]);
            MailMessage msg = new MailMessage(_from, to);

            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("New Order Detail");
            messageBuilder.AppendLine("User Details:");
            messageBuilder.AppendLine($"User Id : {orderDetails.User.UserId}, Name : {orderDetails.User.Name}");

            messageBuilder.AppendLine("Product Details:");
            messageBuilder.AppendLine($"Product Id : {orderDetails.ProductId}, Quantity : {orderDetails.Quantity}");

            messageBuilder.AppendLine($"Delivery Address: {orderDetails.User.DeliveryAddress}");

            msg.Body = messageBuilder.ToString();
            msg.Subject = "New Order Details";
            try
            {
                _mailClient.Send(msg);
                result = true;
            }
            catch (Exception)
            {
                
            }
            return result;
        }
    }
}
