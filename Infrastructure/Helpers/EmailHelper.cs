using Domain.DTOs.EmailDto;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Infrastructure.Helpers;

public static class EmailHelper
{
    public static async Task SendEmail(SendEmail email,
        EmailSettings emailSettings)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(emailSettings.FromName, emailSettings.FromEmail));
        message.To.Add(MailboxAddress.Parse(email.To));
        message.Subject = email.Subject;
        var body = new BodyBuilder {HtmlBody = email.Body};
        message.Body = body.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            if (emailSettings.UseSsl)
            {
                await client.ConnectAsync(emailSettings.Host, emailSettings.Port, SecureSocketOptions.SslOnConnect);
            }
            else if (emailSettings.UseStartTls)
            {
                await client.ConnectAsync(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            }
            else
            {
                await client.ConnectAsync(emailSettings.Host, emailSettings.Port, SecureSocketOptions.None);
            }

            if (!string.IsNullOrWhiteSpace(emailSettings.UserName))
            {
                await client.AuthenticateAsync(emailSettings.UserName, emailSettings.Password);
            }

            await client.SendAsync(message);
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}
