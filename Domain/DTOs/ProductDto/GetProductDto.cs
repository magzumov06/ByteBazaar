using Domain.DTOs.ReviewDto;
using Domain.Entities;

namespace Domain.DTOs.ProductDto;

public class GetProductDto
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal AverageRating { get; set; }
    public int RatingCount { get; set; }
}