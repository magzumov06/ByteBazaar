﻿using System.Net;
using Domain.DTOs.CategoryDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data; 
using Infrastructure.Interfaces.IProducts___ICategories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Infrastructure.Services.Products___Categories;

public class CategoryService(DataContext context) : ICategoryService
{
    public async Task<Responce<string>> CreateCategory(CreateCategoryDto category)
    {
        try
        {
            Log.Information("Creating category");
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
                 Log.Information("Category created");
             }
             else
             {
                 Log.Fatal("Failed to create category");
             }
             return res > 0
                 ? new Responce<string>(HttpStatusCode.Created,"Category created")
                 : new Responce<string>(HttpStatusCode.BadRequest,"Category not created");
        }
        catch (Exception e)
        {
            Log.Error("Error in CreateCategory");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<GetCategoryDto>>> GetCategory()
    {
        try
        {
            Log.Information("Getting categories");
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
            Log.Error("Error in GetCategory");
            return new Responce<List<GetCategoryDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}