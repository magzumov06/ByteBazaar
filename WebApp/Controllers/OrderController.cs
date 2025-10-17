using Domain.DTOs.OrderDto;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService service): Controller
{
    [HttpPost]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> CreateOrder( CreateOrderDto create)
    {
        var res =  await service.CreateOrder(create);
        return Ok(res);
    }
    
    [HttpPut]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> UpdateOrder(UpdateOrderDto dto)
    {
        var res = await service.UpdateStatusOrder(dto);
        return Ok(res);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> GetOrders([FromQuery]OrderFilter filter)
    {
        var res =  await service.GetOrders(filter);
        return Ok(res);
    }

    [HttpGet("api/User/{GetUser}")]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> GetOrders(int userId)
    {
        var res = await service.GetOrdersByUserId(userId);
        return Ok(res);
    }

    [HttpGet("api/orders/ById")]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var res = await service.GetOrderById(id);
        return Ok(res);
    }

}