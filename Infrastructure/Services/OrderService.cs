using System.Net;
using Domain.DTOs.OrderDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class OrderService(DataContext context): IOrderService
{
    public async Task<Responce<string>> CreateOrder(CreateOrderDto create)
    {
        try
        {
            Log.Information("Creating order");
            var cartItem =  context.CartItems
                .Where(c => c.UserId == create.UserId);
            if(!cartItem.Any()) return new Responce<string>(HttpStatusCode.BadRequest,"CartItems not found");
            var order = new Order()
            {
                UserId = create.UserId,
                OrderDate = DateTime.UtcNow,
                Status = create.Status,
                Address = create.Address,
                PaymentMethod = create.PaymentMethod,
            };
            await context.Orders.AddAsync(order);
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("Order created");
            }
            else
            {
                Log.Fatal("Order create failed");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Order created")
                : new Responce<string>(HttpStatusCode.BadRequest,"Order could not be created");
        }
        catch (Exception e)
        {
            Log.Error("Error in CreateOrder");
           return new Responce<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Responce<string>> UpdateStatusOrder(UpdateOrderDto update)
    {
        try
        {
            Log.Information("Updating order");
            var update1 = await context.Orders.FirstOrDefaultAsync(x => x.Id == update.Id);
            if (update1 == null) return new Responce<string>(HttpStatusCode.NotFound, "Order not found");
            update1.Status = update.Status;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            { 
                Log.Information("Order updated");
            }
            else
            {
                Log.Fatal("Order update failed");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Order updated")
                : new Responce<string>(HttpStatusCode.BadRequest, "Order could not be updated");
        }
        catch (Exception e)
        {
            Log.Error("Error in UpdateStatusOrder");
            return new Responce<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetOrderDto>>> GetOrders(OrderFilter filter)
    {
        try
        {
            Log.Information("Getting orders");
            var query = context.Orders.AsQueryable();
            if (filter.Id.HasValue)
            {
                query = query.Where(x => x.Id == filter.Id);
            }

            if (!string.IsNullOrEmpty(filter.Address))
            {
                query = query.Where(x => x.Address.Contains(filter.Address));
            }

            if (filter.UserId.HasValue)
            {
                query = query.Where(x => x.UserId == filter.UserId);
            }
            
            if (filter.TotalAmount.HasValue)
            {
                query = query.Where(x => x.TotalAmount == filter.TotalAmount);
            }

            if (filter.PaymentMethod.HasValue)
            {
                query = query.Where(x => x.PaymentMethod == filter.PaymentMethod);
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(x => x.Status == filter.Status);
            }

            var total = await query.CountAsync();
            var skip = (filter.PageNumber -1) * filter.PageSize;
            var orders = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if(orders.Count == 0) return new PaginationResponce<List<GetOrderDto>>(HttpStatusCode.NotFound,"Order not found");
            var dtos = orders.Select(x=>  new GetOrderDto()
            {
                Id = x.Id,
                Address = x.Address,
                PaymentMethod = x.PaymentMethod,
                Status = x.Status,
                TotalAmount = x.TotalAmount,
                OrderItems = x.OrderItems.Select(oi => new OrderItemFilter()
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                }).ToList(),
                OrderDate = x.OrderDate,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new PaginationResponce<List<GetOrderDto>>(dtos, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetOrders");
            return new PaginationResponce<List<GetOrderDto>>(HttpStatusCode.InternalServerError,e.Message);
        }
    }
    public async Task<Responce<List<GetOrderDto>>> GetOrdersByUserId(int userId)
    {
        try
        {
            Log.Information("Getting orders");
            var orders = await context.Orders
                .Include(o=>o.OrderItems)
                .Where(o=> o.UserId == userId)
                .ToListAsync();
            if(orders.Count == 0) return new Responce<List<GetOrderDto>>(HttpStatusCode.NotFound,"Orders not found");
            var dtos = orders.Select(x=>  new GetOrderDto()
            {
                Id = x.Id,
                UserId = x.UserId,
                Address = x.Address,
                PaymentMethod = x.PaymentMethod,
                Status = x.Status,
                TotalAmount = x.OrderItems.Sum(oi => oi.Price),
                OrderItems = x.OrderItems.Select(oi => new OrderItemFilter()
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                }).ToList(),
                OrderDate = x.OrderDate,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new Responce<List<GetOrderDto>>(dtos);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetOrdersByUserId");
            return new Responce<List<GetOrderDto>>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Responce<GetOrderDto>> GetOrderById(int id)
    {
        try
        {
            Log.Information("Getting order");
            var  order = await context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (order == null) return new Responce<GetOrderDto>(HttpStatusCode.NotFound, "Order not found");
            var dto = new GetOrderDto()
            {
                Id = order.Id,
                UserId = order.UserId,
                Address = order.Address,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status,
                TotalAmount = order.OrderItems.Sum(oi => oi.Price),
                OrderItems = order.OrderItems.Select(oi => new OrderItemFilter()
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                }).ToList(),
                OrderDate = order.OrderDate,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
            };
            return new Responce<GetOrderDto>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetOrderById");
            return new Responce<GetOrderDto>(HttpStatusCode.InternalServerError,e.Message);
        }
    }
}