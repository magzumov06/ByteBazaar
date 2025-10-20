using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.CategoryDto;

public class UpdateCategoryDto
{
    public int Id{get;set;}
    [MinLength(3 , ErrorMessage = "Name minimum length is 3")]
    public string Name { get; set; }
    public string Description { get; set; }
}