namespace Domain.DTOs.OrderItemDto;

public class CreateOrderItemDto
{
    public int OrderId{get;set;}
    public int ProductId{get;set;}
    public int Quantity{get;set;}
}