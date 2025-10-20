using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTOs.OrderDto;

public class UpdateOrderDto
{
    public int Id{get;set;}
    public Status Status{get;set;}
}