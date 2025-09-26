
using Domain.DTOs.EmailDto;
using Infrastructure.Interfaces;
using Infrastructure.Services.EmailServices;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SendEmailController(IEmailService service): Controller
{
    [HttpPost]
    public async Task<IActionResult> SendEmail(SendEmail dto)
    {
        await service.SendEmail(dto);
        return Ok();
    }
}