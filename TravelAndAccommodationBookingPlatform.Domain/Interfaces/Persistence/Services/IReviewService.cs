using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IReviewService
{
    Task AddReview(Guid hotelId, Review review);
    Task UpdateReview(Review review);
    Task DeleteReview(Guid hotelId, Guid reviewId);
    Task<Review> GetReviewById(Guid hotelId, Guid reviewId);
    Task<List<Review>> GetReviews(SieveModel sieveModel, Guid hotelId);
}
