using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

public interface IEmailService
{
    Task SendEmailAsync(string emailToSend, string subject, string message);
}