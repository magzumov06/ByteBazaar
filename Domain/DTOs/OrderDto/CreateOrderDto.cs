using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTOs.OrderDto;

public class CreateOrderDto
{
    public int UserID { get; set; }
    public Status Status{get;set;}
    [Required]
    public string Address{get;set;}
    public PaymentMethod PaymentMethod {get;set;}
}