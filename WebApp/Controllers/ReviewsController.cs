using Domain.DTOs.ReviewDto;
using Infrastructure.Interfaces.Reviews___Ratings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReviewsController(IReviewsRatings service): Controller
{
    [HttpPost]
    public async Task<IActionResult> CreateReview(CreateReviewDto dto)
    {
        var res = await service.AddReview(dto);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateReview(UpdateReviewDto dto)
    {
        var res = await service.UpdateReview(dto);
        return Ok(res);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteReview(int userId)
    {
        var res = await service.DeleteReview(userId);
        return Ok(res);
    }
    
    [HttpGet("{my-reviews}")]
    public async Task<IActionResult> GetReviews(int userId)
    {
        var res = await service.GetReviews(userId);
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReviews()
    {
        var res = await service.GetAllReviews();
        return Ok(res);
    }
}