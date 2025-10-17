using Domain.DTOs.EmailDto;
using Infrastructure.Helpers;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.EmailServices;

public class EmailService(IOptions<EmailSettings> options):IEmailService
{
    private readonly EmailSettings _settings = options.Value;
    public async Task SendEmail(SendEmail dto)
    {
        await EmailHelper.SendEmail(dto, _settings);
    }
}