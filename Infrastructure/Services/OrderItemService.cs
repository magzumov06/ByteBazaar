using System.Net;
using Domain.DTOs.OrderItemDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class OrderItemService(DataContext context) : IOrderItemService
{
    public async Task<Responce<string>> CreateOrderItem(CreateOrderItemDto dto)
    {
        try
        {
            Log.Information("Creating order item");
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == dto.ProductId);
            var cartitem = await context.CartItems.FirstOrDefaultAsync(x => x.ProductId == product!.Id);
            if (product == null) return new Responce<string>(HttpStatusCode.NotFound, "Product not found");
            var newOrderItem = new OrderItem()
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Price = product.Price * cartitem!.Quantity,
                Quantity = dto.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            await context.OrderItems.AddAsync(newOrderItem);
            var res =  await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("Order item created");
            }
            else
            {
                Log.Fatal("Order item could not be created");
            }
            return res > 0 
                ? new Responce<string>(HttpStatusCode.Created, "Order item created")
                : new Responce<string>(HttpStatusCode.BadRequest, "Order item not created");
        }
        catch (Exception e)
        {
            Log.Error("Error in CreateOrderItem");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateOrderItem(UpdateOrderItemDto dto)
    {
        try
        {
            Log.Information("Updating order item");
            var  orderItem = await context.OrderItems.FirstOrDefaultAsync(x=>x.Id == dto.Id);
            if(orderItem == null) return new Responce<string>(HttpStatusCode.NotFound, "Order item not found");
            orderItem.Quantity = dto.Quantity;
            orderItem.UpdatedAt = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("Order item updated");
            }
            else
            {
                Log.Fatal("Order item could not be updated");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Order item updated")
                : new Responce<string>(HttpStatusCode.BadRequest, "Order item not updated");
        }
        catch (Exception e)
        {
            Log.Error("Error in UpdateOrderItem");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteOrderItem(int id)
    {
        try
        {
            Log.Information("Deleting order item");
            var orderItem = await context.OrderItems.FirstOrDefaultAsync(x => x.Id == id);
            if(orderItem == null) return new Responce<string>(HttpStatusCode.NotFound, "Order item not found");
            orderItem.IsDeleted = true;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("Order item deleted");
            }
            else
            {
                Log.Fatal("Order item could not be deleted");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Order item deleted")
                : new Responce<string>(HttpStatusCode.BadRequest, "Order item not deleted");
        }
        catch (Exception e)
        {
            Log.Error("Error in DeleteOrderItem");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetOrderItemDto>> GetOrderItemById(int id)
    {
        try
        {
            Log.Information("Getting order item");
            var orderItem = await context.OrderItems.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if(orderItem == null) return new Responce<GetOrderItemDto>(HttpStatusCode.NotFound, "Order item not found");
            var dto = new GetOrderItemDto()
            {
                Id = orderItem.Id,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                Price = orderItem.Price,
                Quantity = orderItem.Quantity,
                CreatedAt = orderItem.CreatedAt,
                UpdatedAt = orderItem.UpdatedAt,
            };
            return new Responce<GetOrderItemDto>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetOrderItem");
            return new Responce<GetOrderItemDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetOrderItemDto>>> GetOrderItems(OrderItemFilter filter)
    {
        try
        {
            Log.Information("Getting order items");
            var query = context.OrderItems.AsQueryable();
            if (filter.Id.HasValue)
            {
                query = query.Where(x => x.OrderId == filter.Id);
            }

            if (filter.OrderId.HasValue)
            {
                query = query.Where(x => x.OrderId == filter.OrderId);
            }

            if (filter.ProductId.HasValue)
            {
                query = query.Where(x => x.ProductId == filter.ProductId);
            }

            if (filter.Quantity.HasValue)
            {
                query = query.Where(x => x.Quantity == filter.Quantity);
            }

            if (filter.Price.HasValue)
            {
                query = query.Where(x => x.Price == filter.Price);
            }
            query =  query.Where(x=>x.IsDeleted == false);
            var total = await query.CountAsync();
            var skip =  (filter.PageNumber - 1) * filter.PageSize;
            var orderItems = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if(orderItems.Count == 0) return new PaginationResponce<List<GetOrderItemDto>>(HttpStatusCode.NotFound,"Order item not found");
            var dtos = orderItems.Select(x=> new GetOrderItemDto()
            {
                Id = x.Id,
                OrderId = x.OrderId,
                ProductId = x.ProductId,
                Price = x.Price,
                Quantity = x.Quantity,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new PaginationResponce<List<GetOrderItemDto>>(dtos, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetOrderItems");
            return new PaginationResponce<List<GetOrderItemDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}