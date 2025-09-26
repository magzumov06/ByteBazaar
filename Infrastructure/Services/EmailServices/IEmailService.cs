using Domain.DTOs.EmailDto;
using MimeKit.Text;

namespace Infrastructure.Services.EmailServices;

public interface IEmailService
{
    Task SendEmail(SendEmail dto);
}