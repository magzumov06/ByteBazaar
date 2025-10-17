using Domain.Entities;
using Domain.Enums;

namespace Domain.DTOs.OrderDto;

public class GetOrderDto:UpdateOrderDto
{
    public int Id{get;set;}
    public int UserId{get;set;}
    public DateTime OrderDate{get;set;}
    public decimal TotalAmount{get;set;}
    public Status Status{get;set;}
    public string Address{get;set;}
    public PaymentMethod PaymentMethod {get;set;}
    public DateTime CreatedAt{get;set;}
    public DateTime UpdatedAt{get;set;}
    public List<Filters.OrderItemFilter> OrderItems { get; set; }
}