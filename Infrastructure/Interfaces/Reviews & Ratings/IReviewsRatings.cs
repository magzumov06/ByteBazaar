using Domain.DTOs.ReviewDto;
using Infrastructure.Responces;

namespace Infrastructure.Interfaces.Reviews___Ratings;

public interface IReviewsRatings
{
    Task<Responce<string>> AddReview(CreateReviewDto dto);
    Task<Responce<string>> UpdateReview(UpdateReviewDto dto);
    Task<Responce<string>> DeleteReview(Guid userId);
    Task<Responce<List<GetReviewDto>>> GetReviews(Guid userId);
    Task<Responce<List<GetReviewDto>>> GetAllReviews();
}