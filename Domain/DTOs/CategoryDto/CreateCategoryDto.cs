using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.CategoryDto;

public class CreateCategoryDto
{
    [Required]
    [MinLength(3 , ErrorMessage = "Name minimum length is 3")]
    public required string Name { get; set; }
    public string? Description { get; set; }
}