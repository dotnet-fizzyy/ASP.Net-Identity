using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Startup.ApplicationSettings;

using MailKit.Net.Smtp;

using MimeKit;
using MimeKit.Text;

using System.Threading.Tasks;

namespace IdentityWebApi.Infrastructure.Net.Services;

/// <inheritdoc cref="IEmailService" />
public class EmailService : IEmailService
{
    private readonly AppSettings appSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="appSettings"><see cref="AppSettings"/>.</param>
    public EmailService(AppSettings appSettings)
    {
        this.appSettings = appSettings;
    }

    /// <inheritdoc/>
    public async Task SendEmailAsync(string emailToSend, string subject, string message)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(
                this.appSettings.SmtpClientSettings.EmailName,
                this.appSettings.SmtpClientSettings.EmailAddress)
        );
        email.To.Add(new MailboxAddress(string.Empty, emailToSend));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = message,
        };

        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(
            this.appSettings.SmtpClientSettings.Host,
            this.appSettings.SmtpClientSettings.Port,
            this.appSettings.SmtpClientSettings.UseSsl
        );

        await smtpClient.AuthenticateAsync(
            this.appSettings.SmtpClientSettings.EmailAddress,
            this.appSettings.SmtpClientSettings.Password
        );
        await smtpClient.SendAsync(email);

        await smtpClient.DisconnectAsync(true);
    }
}
