using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IReviewRepository
{
    Task AddReview(Review review);
    Task UpdateReview(Review review);
    Task<Review?> GetReview(Guid hotelId, Guid reviewId);
    Task<List<Review>> GetAllReviews(SieveModel sieveModel, Guid hotelId);
    Task DeleteReview(Review review);
}
