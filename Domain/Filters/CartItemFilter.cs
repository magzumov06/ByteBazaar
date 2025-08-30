namespace Domain.Filters;

public class CartItemFilter:BaseFilter
{
    public int? UserId { get; set; }
    public int? ProductId { get; set; }
    public int? Quantity { get; set; }
}