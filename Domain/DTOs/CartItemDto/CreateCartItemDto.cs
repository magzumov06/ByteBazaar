namespace Domain.DTOs.CartItemDto;

public class CreateCartItemDto
{
    public Guid UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}