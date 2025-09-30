using Domain.DTOs.EmailDto;

namespace Infrastructure.Services.EmailServices;

public interface IEmailService
{
   Task SendEmail(SendEmail dto);
}