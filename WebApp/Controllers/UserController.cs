using Domain.DTOs.UserDto;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService service) :Controller
{
    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserDto dto)
    {
        var res = await service.UpdateUser(dto);
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var res = await service.GetUser(id);
        return Ok(res);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilter filter)
    {
        var res  = await service.GetUsers(filter);
        return Ok(res);
    }
}