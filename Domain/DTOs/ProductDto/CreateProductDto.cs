using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.ProductDto;

public class CreateProductDto
{
    [Required]
    [MinLength(3 , ErrorMessage = "Name minimum length must be 3" )]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public string? ImageUrl { get; set; }
}