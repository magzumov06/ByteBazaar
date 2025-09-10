using System.Net;
using Domain.DTOs.ReviewDto;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Interfaces.Reviews___Ratings;
using Infrastructure.Responces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Reviews___Ratings;

public class ReviewsRatings(DataContext context, ILogger<ReviewsRatings>logger):IReviewsRatings
{
    public async Task<Responce<string>> AddReview(CreateReviewDto dto)
    {
        try
        {
            logger.LogInformation("Adding review");
            var isBuy =  context.Orders
                .Include(o => o.OrderItems)
                .AnyAsync(o => o.UserID == dto.UserId && o.Status == Status.Delivered
                                                      && o.OrderItems.Any(oi => oi.ProductId == dto.ProductId)
                );
            if (isBuy != null)
            {
                return new Responce<string>(HttpStatusCode.BadRequest,
                    "Шумо ин маҳсулотро ҳоло нахаридаед.Аз ин сабаб шарҳ гузошта наметавонед!");
            }
            var comment = await context.Reviews.FirstOrDefaultAsync(x=>x.UserId == dto.UserId && x.ProductId == dto.ProductId);
            if (comment != null)
            {
                return new Responce<string>(HttpStatusCode.BadRequest,
                    "Шумо алакай ба ин маҳсулот шарҳ гузоштаед!");
            }

            var review = new Review()
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId,
                Comment = dto.Comment,
                Rating = dto.Rating,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await context.Reviews.AddAsync(review);
            
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == dto.ProductId);
            if(product == null) return new Responce<string>(HttpStatusCode.NotFound,"Product not found");
            product.RatingCount += 1;
            var average = await context.Reviews
                .Where(r=> r.ProductId == product.Id)
                .AverageAsync(x => x.Rating);
            product.AverageRating = average;
            
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                logger.LogInformation("Adding review");
            }
            else
            {
                logger.LogError("Adding review fail");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Review added")
                : new Responce<string>(HttpStatusCode.NotFound,"Review not added");
        }
        catch (Exception e)
        {
            logger.LogError("Error adding review");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateReview(UpdateReviewDto dto)
    {
        try
        {
            logger.LogInformation("Updating review");
            var  review = await context.Reviews.FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.ProductId == dto.ProductId);
            if (review == null) return new Responce<string>(HttpStatusCode.NotFound,"Review not found");
            review.Comment = dto.Comment;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                logger.LogInformation("Updating review");
            }
            else
            {
                logger.LogError("Updating review fail");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Review updated")
                : new Responce<string>(HttpStatusCode.BadRequest,"Review not updated");
        }
        catch (Exception e)
        {
            logger.LogError("Error updating review");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteReview(Guid userId)
    {
        try
        {
            logger.LogInformation("Deleting review");
            var review = await context.Reviews.FirstOrDefaultAsync(x => x.UserId == userId);
            if (review == null) return new Responce<string>(HttpStatusCode.NotFound,"Review not found");
            context.Reviews.Remove(review);
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                logger.LogInformation("Deleting review");
            }
            else
            {
                logger.LogError("Deleting review fail");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Review deleted")
                : new Responce<string>(HttpStatusCode.NotFound,"Review not deleted");
        }
        catch (Exception e)
        {
            logger.LogError("Error deleting review");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<GetReviewDto>>> GetReviews(Guid userId)
    {
        try
        {
            logger.LogInformation("Getting reviews");
            var reviews = await context.Reviews.Where(x => x.UserId == userId).ToListAsync();
            if(reviews.Count == 0) return new Responce<List<GetReviewDto>>(HttpStatusCode.NotFound, "Review not found");
            var dto =  reviews.Select(x=>new GetReviewDto()
            {
                Id = x.Id,
                UserId = x.UserId,
                Comment = x.Comment,
                Rating = x.Rating,
                ProductId = x.ProductId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                
            }).ToList();
            return new Responce<List<GetReviewDto>>(dto);
        }
        catch (Exception e)
        {
            logger.LogError("Error getting reviews");
            return new Responce<List<GetReviewDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<GetReviewDto>>> GetAllReviews()
    {
        try
        {
            logger.LogInformation("Getting all reviews");
            var reviews = await context.Reviews.ToListAsync();
            if(reviews.Count == 0) return new Responce<List<GetReviewDto>>(HttpStatusCode.NotFound, "Reviews not found");
            var dtos = reviews.Select(x=> new GetReviewDto()
            {
                Id = x.Id,
                UserId = x.UserId,
                Comment = x.Comment,
                Rating = x.Rating,
                ProductId = x.ProductId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new Responce<List<GetReviewDto>>(dtos);
        }
        catch (Exception e)
        {
            logger.LogError("Error getting reviews");
            return new Responce<List<GetReviewDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}