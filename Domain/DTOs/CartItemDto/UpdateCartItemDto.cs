namespace Domain.DTOs.CartItemDto;

public class UpdateCartItemDto:CreateCartItemDto
{
    public int Id{get;set;}
    public bool IsDeleted{get;set;}
}