using Domain.DTOs.OrderDto;
using Domain.Filters;
using Infrastructure.Interfaces;
using Infrastructure.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService service): Controller
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateOrder( CreateOrderDto create)
    {
        var res =  await service.CreateOrder(create);
        return Ok(res);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateOrder(UpdateOrderDto dto)
    {
        var res = await service.UpdateStatusOrder(dto);
        return Ok(res);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery]OrderFilter filter)
    {
        var res =  await service.GetOrders(filter);
        return Ok(res);
    }

    [HttpGet("api/User/{GetUser}")]
    public async Task<IActionResult> GetOrders(Guid userId)
    {
        var res = await service.GetOrders(userId);
        return Ok(res);
    }

    [HttpGet("api/orders/ById")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var res = await service.GetOrderById(id);
        return Ok(res);
    }

}