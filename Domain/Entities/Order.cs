using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Order : BaseEntities
{
    public int Id{get;set;}
    public int UserId{get;set;}
    public DateTime OrderDate{get;set;}
    public decimal TotalAmount{get;set;}
    public Status Status{get;set;}
    [Required]
    public required string Address{get;set;}
    public PaymentMethod PaymentMethod {get;set;}
    public List<OrderItem>? OrderItems { get; set; }
    public User? User { get; set; }
}
