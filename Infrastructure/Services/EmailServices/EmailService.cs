// using Domain.DTOs.EmailDto;
// using MailKit.Security;
// using Microsoft.Extensions.Configuration;
// using MimeKit;
// using MimeKit.Text;
// using SmtpClient = MailKit.Net.Smtp.SmtpClient;
//
// namespace Infrastructure.Services.EmailServices;
//
// public class EmailService(EmailConfiguration emailConfiguration, IConfiguration configuration):IEmailService
// {
//     public async Task SendEmail(EmailMessageDto dto, TextFormat format)
//     {
//         var emailMessage = CreateEmailMessage(dto, format);
//         await SendAsync(emailMessage);
//     }
//     
//     private MimeMessage CreateEmailMessage(EmailMessageDto message, TextFormat format)
//     {
//         var emailMessage = new MimeMessage();
//         emailMessage.From.Add(new MailboxAddress(configuration["EmailConfiguration:DisplayName"], emailConfiguration.From));
//         emailMessage.To.AddRange(message.To);
//         emailMessage.Subject = message.Subject;
//         emailMessage.Body = new TextPart(format) { Text = message.Content };
//         return emailMessage;
//     }
//     
//     private async Task SendAsync(MimeMessage mailMessage)
//     {
//         using var client = new SmtpClient();
//         try
//         {
//             await client.ConnectAsync(emailConfiguration.SmtpServer, emailConfiguration.Port, SecureSocketOptions.StartTls);
//             client.AuthenticationMechanisms.Remove("OAUTH2");
//             await client.AuthenticateAsync(emailConfiguration.Username, emailConfiguration.Password);
//             await client.SendAsync(mailMessage);
//         }
//         finally
//         {
//             await client.DisconnectAsync(true);
//         }
//     }
//
// }