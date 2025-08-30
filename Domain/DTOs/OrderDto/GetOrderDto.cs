namespace Domain.DTOs.OrderDto;

public class GetOrderDto:UpdateOrderDto
{
    public DateTime CreatedAt{get;set;}
    public DateTime UpdatedAt{get;set;}
}