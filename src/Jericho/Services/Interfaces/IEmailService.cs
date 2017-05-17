namespace Jericho.Services.Interfaces
{
    using System.Threading.Tasks;

    using Jericho.Options;

    public interface IEmailService
    {
        MailJetOptions MailJetOptions { get; set; }

        Task SendEmailAsync(string email, string subject, string message);
    }
}
