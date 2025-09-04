using Domain.DTOs.CartItemDto;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController(ICartService service):Controller
{
    [HttpPost]
    public async Task<IActionResult> AddCartItem(CreateCartItemDto dto)
    {
        var res = await service.AddToCart(dto);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCartItem(UpdateCartItemDto dto)
    {
        var res = await service.UpdateCart(dto);
        return Ok(res);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCart(int Id)
    {
        var res = await service.DeleteCart(Id);
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> GetCart(Guid UserId)
    {
        var res = await service.GetCartItem(UserId);
        return Ok(res);
    }
}