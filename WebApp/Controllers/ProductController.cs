using Domain.DTOs.ProductDto;
using Domain.Filters; 
using Infrastructure.Interfaces.IProducts___ICategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService service):ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProduct(CreateProductDto product)
    {
        var res = await service.CreateProduct(product);
        return Ok(res);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct([FromForm]UpdateProductDto product)
    {
        var res = await service.UpdateProduct(product);
        return Ok(res);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var res = await service.DeleteProduct(id);
        return Ok(res);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProducts([FromQuery] ProductFilter filter)
    {
        var res = await service.GetProducts(filter);
        return Ok(res);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductById(int id)
    {
        var res = await service.GetProductById(id);
        return Ok(res);
    }
    
}