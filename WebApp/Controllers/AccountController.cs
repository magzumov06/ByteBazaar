using Domain.DTOs.Account;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] Register request)
    {
        var res = await service.Register(request);
        return Ok(res);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginDto request)
    {
        var res = await service.Login(request);
        return Ok(res);
    }

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassword changePasswordDto)
    {
        var res = await service.ChangePassword(changePasswordDto);
        return Ok(res);
    }
}