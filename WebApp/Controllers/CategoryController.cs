using Domain.DTOs.CategoryDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService  service):ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto category)
    {
        var  res = await service.CreateCategory(category);
        return Ok(res);
    }

    [HttpGet("{categories}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategories()
    {
        var res = await service.GetCategory();
        return Ok(res);
    }
}