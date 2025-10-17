namespace Domain.DTOs.OrderItemDto;

public class GetOrderItemDto : UpdateOrderItemDto
{
    public DateTime CreatedAt{get;set;}
    public DateTime UpdatedAt{get;set;}
}