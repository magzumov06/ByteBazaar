namespace Domain.DTOs.CartItemDto;

public class GetCartItemDto:CreateCartItemDto
{
    public int Id{get;set;}
    public DateTime CreatedAt{get;set;}
    public DateTime UpdatedAt{get;set;}
}