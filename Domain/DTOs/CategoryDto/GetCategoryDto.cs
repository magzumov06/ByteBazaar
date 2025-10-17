namespace Domain.DTOs.CategoryDto;

public class GetCategoryDto:UpdateCategoryDto
{
    public DateTime CreatedAt{get;set;}
    public DateTime UpdatedAt{get;set;}
}