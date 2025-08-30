using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTOs.OrderDto;

public class UpdateOrderDto
{
    public int Id{get;set;}
    public int UserId{get;set;}
    public DateTime OrderDate{get;set;}
    public decimal TotalAmount{get;set;}
    public Status Status{get;set;}
    public string Address{get;set;}
    public PaymentMethod PaymentMethod {get;set;}
    public bool IsDeleted{get;set;}
}