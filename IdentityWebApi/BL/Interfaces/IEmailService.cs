using System.Threading.Tasks;

namespace IdentityWebApi.BL.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailToSend, string subject, string message);
    }
}