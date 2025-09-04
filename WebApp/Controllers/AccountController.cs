using Domain.DTOs.Account;
using Infrastructure.Interfaces;
using Infrastructure.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService service) : Controller
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromForm] Register request)
    {
        var res = await service.Register(request);
        return Ok(res);
    }


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto request)
    {
        var res = await service.Login(request);
        return Ok(res);
    }

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassword changePasswordDto, Guid id)
    {
        var res = await service.ChangePassword(changePasswordDto, id);
        return Ok(res);
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        var res = await service.ResetPassword(resetPasswordDto);
        return Ok(res);
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword forgotPassword)
    {
        var res = await service.ForgotPasswordCodeGenerator(forgotPassword);
        return Ok(res);
    }
}