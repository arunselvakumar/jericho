namespace Jericho.Services
{
    using System.Threading.Tasks;

    using Jericho.Services.Interfaces;

    using Mailjet.Client;
    using Mailjet.Client.Resources;

    using Microsoft.Extensions.Options;

    using Newtonsoft.Json.Linq;

    using Options;

    public class EmailService : IEmailService
    {
        public EmailService(IOptions<MailJetOptions> mailJetOptions)
        {
            this.MailJetOptions = mailJetOptions.Value;
        }

        public MailJetOptions MailJetOptions { get; set; }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new MailjetClient(this.MailJetOptions.APIKey, this.MailJetOptions.SecretKey);
            var request =
                new MailjetRequest { Resource = Send.Resource, }.Property(Send.FromEmail, "b.arunselvakumar@outlook.com")
                    .Property(Send.FromName, "Jericho")
                    .Property(Send.Subject, subject)
                    .Property(Send.TextPart, message)
                    .Property(Send.HtmlPart, message)
                    .Property(Send.Recipients, new JArray { new JObject { { "Email", email } } });

            await client.PostAsync(request);            
        }
    }
}