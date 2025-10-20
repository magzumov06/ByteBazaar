using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class Product : BaseEntities
{
    public int Id { get; set; }
    [Required]
    [MinLength(3 , ErrorMessage = "Name minimum length must be 3" )]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public string? ImageUrl { get; set; }
    public decimal AverageRating { get; set; }
    public int RatingCount { get; set; }
    
    public List<Review>? Reviews { get; set; }
    public List<OrderItem>? OrderItems { get; set; }
    public List<CartItem>? CartItems { get; set; }
    public Category? Category { get; set; }
}