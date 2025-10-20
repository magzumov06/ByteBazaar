using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.ReviewDto;

public class UpdateReviewDto
{
    public int Id { get; set; }
    public int UserId{get;set;}
    public int ProductId{get;set;}
    [Range(1,5,ErrorMessage = "Rating must be between 1 and 5")]
    public decimal Rating {get;set;}
    public string? Comment  { get; set; }
}