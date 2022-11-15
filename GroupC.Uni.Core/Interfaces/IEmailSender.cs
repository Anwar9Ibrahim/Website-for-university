using System.Threading.Tasks;

namespace GroupC.Uni.Core.Interfaces
{

    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
