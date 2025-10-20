using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.ReviewDto;

public class CreateReviewDto
{
    public int UserId{get;set;}
    public int ProductId{get;set;}
    
    [Range(1,5,ErrorMessage = "Rating must be between 1 and 5")]
    [Required]
    public decimal Rating {get;set;}
    public string? Comment  { get; set; }
    
}