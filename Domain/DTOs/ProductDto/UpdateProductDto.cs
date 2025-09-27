using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.ProductDto;

public class UpdateProductDto
{
    public int Id { get; set; }
    [MinLength(3 , ErrorMessage = "Name minimum length must be 3" )]
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public IFormFile? ImageUrl { get; set; }
}