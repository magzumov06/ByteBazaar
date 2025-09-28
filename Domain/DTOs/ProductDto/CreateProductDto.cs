using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.ProductDto;

public class CreateProductDto
{
    [Required]
    [MinLength(3 , ErrorMessage = "Name minimum length must be 3" )]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
    public required int CategoryId { get; set; }
    public IFormFile? ImageUrl { get; set; }
}