using Domain.DTOs.OrderDto;
using Domain.Filters;
using Domain.Responces;
using Infrastructure.Responces;

namespace Infrastructure.Interfaces;

public interface IOrderService
{
    Task<Responce<string>> CreateOrder(CreateOrderDto create);
    Task<Responce<string>> UpdateStatusOrder(UpdateOrderDto update);
    Task<PaginationResponce<List<GetOrderDto>>> GetOrders(OrderFilter filter);
    Task<Responce<List<GetOrderDto>>> GetOrders(int userId);
    Task<Responce<GetOrderDto>> GetOrderById(int id);
}