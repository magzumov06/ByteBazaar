namespace Domain.DTOs.ReviewDto;

public class GetReviewDto:UpdateReviewDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}