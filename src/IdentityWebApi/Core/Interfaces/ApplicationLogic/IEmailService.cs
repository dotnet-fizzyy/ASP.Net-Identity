using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

/// <summary>
/// Email services.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends email with specific subject and message.
    /// </summary>
    /// <param name="emailToSend">Destination email.</param>
    /// <param name="subject">Email subject.</param>
    /// <param name="message">Email message content.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SendEmailAsync(string emailToSend, string subject, string message);
}