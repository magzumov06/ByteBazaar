namespace Domain.Entities;

public class CartItem:BaseEntities
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public User? User { get; set; }
    public Product? Product { get; set; }
}
