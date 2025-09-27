using System.Net;
using Domain.DTOs.ProductDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.FileStorage;
using Infrastructure.Interfaces.IProducts___ICategories;
using Infrastructure.Responces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Products___Categories;

public class ProductService(DataContext context, 
    ILogger<ProductService> logger,
    IFileStorage file) : IProductService
{
    public async Task<Responce<string>> CreateProduct(CreateProductDto create)
    {
        try
        { 
            logger.LogInformation("Creating a new product");
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
                logger.LogInformation("Product created");
            }
            else
            {
                logger.LogError("Product not created");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.Created,"Product created")
                : new Responce<string>(HttpStatusCode.BadRequest,"Product not created");
        }
        catch (Exception e)
        {
            logger.LogError("Error creating product");
            return new Responce<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Responce<string>> UpdateProduct(UpdateProductDto update)
    {
        try
        {
            logger.LogInformation("Updating a new product");
            var product = await context.Products.FirstOrDefaultAsync(x=> x.Id == update.Id);
            if (product == null) return new Responce<string>(HttpStatusCode.NotFound,"Product not found");
            if (update.ImageUrl != null)
            {
                await file.DeleteFile(product?.ImageUrl!);
            }
            product.Name = update.Name;
            product.Description = update.Description;
            product.Price = update.Price;
            product.Quantity = update.Quantity;
            product.CategoryId = update.CategoryId;
            product.ImageUrl = await file.SaveFile(update.ImageUrl!,"Image");
            product.UpdatedAt = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                logger.LogInformation("Product updated");
            }
            else
            {
                logger.LogError("Product not updated");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Product updated")
                : new Responce<string>(HttpStatusCode.BadRequest,"Product not updated");
        }
        catch (Exception e)
        {
            logger.LogError("Error updating product");
            return new Responce<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Responce<string>> DeleteProduct(int id)
    {
        try
        {
            logger.LogInformation("Deleting a product");
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return new Responce<string>(HttpStatusCode.NotFound,"Product not found");
            product.IsDeleted = true;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                logger.LogInformation("Product deleted");
            }
            else
            {
                logger.LogError("Product not deleted");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Product deleted")
                : new Responce<string>(HttpStatusCode.BadRequest,"Product not deleted");
        }
        catch (Exception e)
        {
            logger.LogError("Error deleting product");
            return new Responce<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Responce<GetProductDto>> GetProductById(int id)
    {
        try
        {
            logger.LogInformation("Getting a product");
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
            logger.LogError("Error getting product");
            return new Responce<GetProductDto>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetProductDto>>> GetProducts(ProductFilter filter)
    {
        try
        {
            logger.LogInformation("Getting products");
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
            logger.LogError("Error getting products");
            return new PaginationResponce<List<GetProductDto>>(HttpStatusCode.InternalServerError,e.Message);
        }
    }
}