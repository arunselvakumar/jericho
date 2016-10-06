namespace Jericho.Services.Interfaces
{
    using Jericho.Options;
    using System.Threading.Tasks;

    public interface IEmailService
    {
        SendGridOptions SendGridOptions { get; set; }

        Task SendEmailAsync(string email, string subject, string message);
    }
}
