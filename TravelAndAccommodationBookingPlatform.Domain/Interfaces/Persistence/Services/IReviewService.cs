using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IReviewService
{
    Task AddReview(Review review);
    Task UpdateReview(Review review);
    Task DeleteReview(Guid reviewId);
    Task<Review> GetReviewById(Guid reviewId);
    Task<List<Review>> GetReviews(SieveModel sieveModel);
}
