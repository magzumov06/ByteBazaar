using System.Net;
using Domain.DTOs.ProductDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.FileStorage;
using Infrastructure.Interfaces.IProducts___ICategories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services.Products___Categories;

public class ProductService(DataContext context, 
    IFileStorage file) : IProductService
{
    public async Task<Responce<string>> CreateProduct(CreateProductDto create)
    {
        try
        { 
            Log.Information("Creating a new product");
            var newProduct = new Product()
            {
                Name = create.Name,
                Description = create.Description,
                Price = create.Price,
                Quantity = create.Quantity,
                CategoryId = create.CategoryId,
                AverageRating = 0,
                RatingCount = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false,
            };
            if (create.ImageUrl != null)
            {
                newProduct.ImageUrl = await file.SaveFile(create.ImageUrl,"Image");
            }
            await context.Products.AddAsync(newProduct);
            var res =  await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("Product created");
            }
            else
            {
                Log.Fatal("Product not created");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.Created,"Product created")
                : new Responce<string>(HttpStatusCode.BadRequest,"Product not created");
        }
        catch (Exception e)
        {
            Log.Error("Error in CreateProduct");
            return new Responce<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Responce<string>> UpdateProduct(UpdateProductDto update)
    {
        try
        {
            Log.Information("Updating a new product");
            var product = await context.Products.FirstOrDefaultAsync(x=> x.Id == update.Id);
            if (product == null) return new Responce<string>(HttpStatusCode.NotFound,"Product not found");
            if (update.ImageUrl != null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    await file.DeleteFile(product.ImageUrl);         
                }
                product.ImageUrl = await file.SaveFile(update.ImageUrl!,"Image");
            }
            product.Name = update.Name;
            product.Description = update.Description;
            product.Price = update.Price;
            product.Quantity = update.Quantity;
            product.CategoryId = update.CategoryId;

            product.UpdatedAt = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("Product updated");
            }
            else
            {
                Log.Fatal("Product not updated");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Product updated")
                : new Responce<string>(HttpStatusCode.BadRequest,"Product not updated");
        }
        catch (Exception e)
        {
            Log.Error("Error in UpdateProduct");
            return new Responce<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Responce<string>> DeleteProduct(int id)
    {
        try
        {
            Log.Information("Deleting a product");
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return new Responce<string>(HttpStatusCode.NotFound,"Product not found");
            product.IsDeleted = true;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("Product deleted");
            }
            else
            {
                Log.Fatal("Product not deleted");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Product deleted")
                : new Responce<string>(HttpStatusCode.BadRequest,"Product not deleted");
        }
        catch (Exception e)
        {
            Log.Error("Error in DeleteProduct");
            return new Responce<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Responce<GetProductDto>> GetProductById(int id)
    {
        try
        {
            Log.Information("Getting a product");
            var product = await context.Products
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false );
            if (product == null) return new Responce<GetProductDto>(HttpStatusCode.NotFound,"Product not found");
            var dto = new GetProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId,
                AverageRating = product.AverageRating,
                RatingCount = product.RatingCount,
                ImageUrl = product.ImageUrl,
                UpdatedAt = product.UpdatedAt,
                CreatedAt = product.CreatedAt
            };
            
            return new Responce<GetProductDto>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetProductById");
            return new Responce<GetProductDto>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetProductDto>>> GetProducts(ProductFilter filter)
    {
        try
        {
            Log.Information("Getting products");
            var query = context.Products.AsQueryable();
            if (filter.Id.HasValue)
            {
                query = query.Where(x => x.Id == filter.Id);
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(x => x.Name.Contains(filter.Name));
            }

            if (!string.IsNullOrEmpty(filter.Description))
            {
                query = query.Where(x => x.Description.Contains(filter.Description));
            }

            if (filter.Price.HasValue)
            {
                query = query.Where(x => x.Price >= filter.Price);
            }

            if (filter.Quantity.HasValue)
            {
                query = query.Where(x => x.Quantity >= filter.Quantity);
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == filter.CategoryId);
            }

            if (filter.AverageRating.HasValue)
            {
                query = query.Where(x => x.AverageRating >= filter.AverageRating);
            }

            if (filter.RatingCount.HasValue)
            {
                query = query.Where(x => x.RatingCount >= filter.RatingCount);
            }
            query = query.Where(x=> x.IsDeleted == false);
            var total = await query.CountAsync();
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            var products = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if(products.Count == 0) return new PaginationResponce<List<GetProductDto>>(HttpStatusCode.NotFound,"Product not found");
            var dtos = products.Select(x => new GetProductDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Quantity = x.Quantity,
                CategoryId = x.CategoryId,
                AverageRating = x.AverageRating,
                RatingCount = x.RatingCount,
                ImageUrl = x.ImageUrl,
                UpdatedAt = x.UpdatedAt,
                CreatedAt = x.CreatedAt
            }).ToList();
            return new PaginationResponce<List<GetProductDto>>(dtos,total,filter.PageNumber,filter.PageSize);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetProducts");
            return new PaginationResponce<List<GetProductDto>>(HttpStatusCode.InternalServerError,e.Message);
        }
    }
}