namespace Domain.DTOs.CartItemDto;

public class GetCartItemDto:UpdateCartItemDto
{
    public DateTime CreatedAt{get;set;}
    public DateTime UpdatedAt{get;set;}
}