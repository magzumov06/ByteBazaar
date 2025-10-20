using Domain.DTOs.ReviewDto;
using Domain.Responces;

namespace Infrastructure.Interfaces.Reviews___Ratings;

public interface IReviewsRatings
{
    Task<Responce<string>> AddReview(CreateReviewDto dto);
    Task<Responce<string>> UpdateReview(UpdateReviewDto dto);
    Task<Responce<string>> DeleteReview(int reviewId);
    Task<Responce<List<GetReviewDto>>> GetReviews(int userId);
    Task<Responce<List<GetReviewDto>>> GetAllReviews();
}