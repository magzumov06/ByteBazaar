using System.Net;
using Domain.DTOs.CategoryDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data; 
using Infrastructure.Interfaces.IProducts___ICategories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Products___Categories;

public class CategoryService(DataContext context, ILogger<CategoryService> logger) : ICategoryService
{
    public async Task<Responce<string>> CreateCategory(CreateCategoryDto category)
    {
        try
        {
            logger.LogInformation("Creating category");
            var newCategory = new Category()
            {
                Name = category.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
             await context.Categories.AddAsync(newCategory);
             var res = await context.SaveChangesAsync();
             if (res > 0)
             {
                 logger.LogInformation("Category created");
             }
             else
             {
                 logger.LogError("Failed to create category");
             }
             return res > 0
                 ? new Responce<string>(HttpStatusCode.Created,"Category created")
                 : new Responce<string>(HttpStatusCode.BadRequest,"Category not created");
        }
        catch (Exception e)
        {
            logger.LogError("Error creating category");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<GetCategoryDto>>> GetCategory()
    {
        try
        {
            logger.LogInformation("Getting categories");
            var categories = await context.Categories.ToListAsync();
            if (categories.Count == 0) return new Responce<List<GetCategoryDto>>(HttpStatusCode.NotFound, "Category not found");
            var dtos = categories.Select(c => new GetCategoryDto()
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
            }).ToList();
            return new Responce<List<GetCategoryDto>>(dtos);
        }
        catch (Exception e)
        {
            logger.LogError("Error getting categories");
            return new Responce<List<GetCategoryDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}