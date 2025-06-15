using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Reviews;

public class ReviewService(IReviewRepository reviewRepository) : IReviewService
{
    public async Task AddReview(Review review)
    {
        await reviewRepository.AddReview(review);
    }

    public async Task UpdateReview(Review review)
    {
        await reviewRepository.UpdateReview(review);
    }

    public async Task DeleteReview(Guid reviewId)
    {
        var review = await GetReviewById(reviewId);
        await reviewRepository.DeleteReview(review);
    }

    public async Task<Review> GetReviewById(Guid reviewId)
    {
        var review = await reviewRepository.GetReview(reviewId);
        if (review is null)
        {
            throw new NotFoundException($"Review with if {reviewId} not found");
        }
        
        return review;
    }

    public async Task<List<Review>> GetReviews(SieveModel sieveModel)
    {
        return await reviewRepository.GetAllReviews(sieveModel);
    }
}
