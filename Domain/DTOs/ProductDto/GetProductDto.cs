using Domain.DTOs.ReviewDto;
using Domain.Entities;

namespace Domain.DTOs.ProductDto;

public class GetProductDto:UpdateProductDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal AverageRating { get; set; }
    public int RatingCount { get; set; }
}