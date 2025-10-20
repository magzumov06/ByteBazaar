namespace Domain.DTOs.OrderItemDto;

public class UpdateOrderItemDto
{
    public int Id { get; set; }
    public int OrderId{get;set;}
    public int ProductId{get;set;}
    public int Quantity{get;set;}
    public decimal Price{get;set;}
}