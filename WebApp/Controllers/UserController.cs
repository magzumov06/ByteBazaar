using Domain.DTOs.UserDto;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IuserService service) :Controller
{
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        var res =  await service.CreateUser(dto);
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilter filter)
    {
        var res  = await service.GetUsers(filter);
        return Ok(res);
    }
}