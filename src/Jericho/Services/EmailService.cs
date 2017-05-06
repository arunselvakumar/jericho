namespace Jericho.Services
{
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    using Jericho.Services.Interfaces;

    using Microsoft.Extensions.Options;

    using Options;

    using SendGrid;

    public class EmailService : IEmailService
    {
        public EmailService(IOptions<SendGridOptions> sendGridOptionsAccessor)
        {
            this.SendGridOptions = sendGridOptionsAccessor.Value;
        }

        public SendGridOptions SendGridOptions { get; set; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(email);
            myMessage.From = new MailAddress("b.arunselvakumar@outlook.com", "Arun Selva Kumar");
            myMessage.Subject = subject;
            myMessage.Text = message;
            myMessage.Html = message;

            var credentials = new NetworkCredential(SendGridOptions.SendGridUser, SendGridOptions.SendGridKey);
            var transportWeb = new SendGrid.Web(credentials);
            return transportWeb.DeliverAsync(myMessage);
        }
    }
}