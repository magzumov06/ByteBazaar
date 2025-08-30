namespace Domain.DTOs.ProductDto;

public class GetProductDto:UpdateProductDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}