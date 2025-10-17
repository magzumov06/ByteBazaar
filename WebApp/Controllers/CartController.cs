using Domain.DTOs.CartItemDto;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController(ICartService service):Controller
{
    [HttpPost]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> AddCartItem(CreateCartItemDto dto)
    {
        var res = await service.AddToCart(dto);
        return Ok(res);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> UpdateCartItem(UpdateCartItemDto dto)
    {
        var res = await service.UpdateCart(dto);
        return Ok(res);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> DeleteCart(int id)
    {
        var res = await service.DeleteCart(id);
        return Ok(res);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> GetCart(int userId)
    {
        var res = await service.GetCartItem(userId);
        return Ok(res);
    }
}