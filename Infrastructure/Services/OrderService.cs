using System.Net;
using Domain.DTOs.OrderDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Responces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class OrderService(DataContext context, ILogger<OrderService> logger): IOrderService
{
    public async Task<Responce<string>> CreateOrder(CreateOrderDto create)
    {
        try
        {
            logger.LogInformation("Creating order");
            var cartItem =  context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == create.UserID);
            if(!cartItem.Any()) return new Responce<string>(HttpStatusCode.BadRequest,"CartItems not found");
            var order = new Order()
            {
                UserID = create.UserID,
                OrderDate = DateTime.UtcNow,
                Status = create.Status,
                Address = create.Address,
                PaymentMethod = create.PaymentMethod,
                TotalAmount = cartItem.Sum(c => c.Product.Price * c.Quantity),
            };
            await context.Orders.AddAsync(order);
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                logger.LogInformation("Order created");
            }
            else
            {
                logger.LogError("Order create failed");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Order created")
                : new Responce<string>(HttpStatusCode.BadRequest,"Order could not be created");
        }
        catch (Exception e)
        {
            logger.LogError("Error creating order");
           return new Responce<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Responce<string>> UpdateStatusOrder(UpdateOrderDto update)
    {
        try
        {
            logger.LogInformation("Updating order");
            var update1 = await context.Orders.FirstOrDefaultAsync(x => x.Id == update.Id);
            if (update1 == null) return new Responce<string>(HttpStatusCode.NotFound, "Order not found");
            update1.Status = update.Status;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                logger.LogInformation("Order updated");
            }
            else
            {
                logger.LogError("Order update failed");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Order updated")
                : new Responce<string>(HttpStatusCode.BadRequest, "Order could not be updated");
        }
        catch (Exception e)
        {
            logger.LogError("Error updating order");
            return new Responce<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetOrderDto>>> GetOrders(OrderFilter filter)
    {
        try
        {
            logger.LogInformation("Getting orders");
            var query = context.Orders.AsQueryable();
            if (filter.Id.HasValue)
            {
                query = query.Where(x => x.Id == filter.Id);
            }

            if (!string.IsNullOrEmpty(filter.Address))
            {
                query = query.Where(x => x.Address.Contains(filter.Address));
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
                OrderItems = x.OrderItems.Select(oi => new OrderItemDto()
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
            logger.LogError("Error getting orders");
            return new PaginationResponce<List<GetOrderDto>>(HttpStatusCode.InternalServerError,e.Message);
        }
    }
    public async Task<Responce<List<GetOrderDto>>> GetOrders(int userId)
    {
        try
        {
            logger.LogInformation("Getting orders");
            var orders = await context.Orders
                .Include(o=>o.OrderItems)
                .Where(o=> o.UserID == userId)
                .ToListAsync();
            if(orders.Count == 0) return new Responce<List<GetOrderDto>>(HttpStatusCode.NotFound,"Orders not found");
            var dtos = orders.Select(x=>  new GetOrderDto()
            {
                Id = x.Id,
                UserId = x.UserID,
                Address = x.Address,
                PaymentMethod = x.PaymentMethod,
                Status = x.Status,
                TotalAmount = x.TotalAmount,
                OrderItems = x.OrderItems.Select(oi => new OrderItemDto()
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
            logger.LogError("Error getting orders");
            return new Responce<List<GetOrderDto>>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Responce<GetOrderDto>> GetOrderById(int id)
    {
        try
        {
            logger.LogInformation("Getting order");
            var  order = await context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (order == null) return new Responce<GetOrderDto>(HttpStatusCode.NotFound, "Order not found");
            var dto = new GetOrderDto()
            {
                Id = order.Id,
                UserId = order.UserID,
                Address = order.Address,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto()
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
            logger.LogError("Error getting order");
            return new Responce<GetOrderDto>(HttpStatusCode.InternalServerError,e.Message);
        }
    }
}