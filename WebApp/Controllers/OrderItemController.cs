﻿using Domain.DTOs.OrderItemDto;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;


[ApiController]
[Route("api/[controller]")]
public class OrderItemController(IOrderItemService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderItemDto dto)
    {
        var res =  await service.CreateOrderItem(dto);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateOrderItemDto dto)
    {
        var res = await service.UpdateOrderItem(dto);
        return Ok(res);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await service.DeleteOrderItem(id);
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var res = await service.GetOrderItemById(id);
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] OrderItemFilter filter)
    {
        var res = await service.GetOrderItems(filter);
        return Ok(res);
    }
}