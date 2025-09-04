using System.Net;
using Domain.DTOs.CategoryDto;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Responces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Products___Categories;

public class CategoryService(DataContext context): ICategoryService
{
    public async Task<Responce<string>> CreateCategory(CreateCategoryDto category)
    {
        try
        {
            var newCategory = new Category()
            {
                Name = category.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
             await context.Categories.AddAsync(newCategory);
             var res = await context.SaveChangesAsync();
             return res > 0
                 ? new Responce<string>(HttpStatusCode.Created,"Category created")
                 : new Responce<string>(HttpStatusCode.BadRequest,"Category not created");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<GetCategoryDto>>> GetCategory()
    {
        try
        {
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
            Console.WriteLine(e);
            throw;
        }
    }
}