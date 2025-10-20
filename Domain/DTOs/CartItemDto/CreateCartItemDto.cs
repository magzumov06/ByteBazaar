namespace Domain.DTOs.CartItemDto;

public class CreateCartItemDto
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}