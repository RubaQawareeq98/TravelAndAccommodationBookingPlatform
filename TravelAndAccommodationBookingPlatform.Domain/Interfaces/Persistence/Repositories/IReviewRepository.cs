using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IReviewRepository
{
    Task AddReview(Review review, CancellationToken cancellationToken);
    Task UpdateReview(Review review, CancellationToken cancellationToken);
    Task<Review?> GetReview(Guid hotelId, Guid reviewId, CancellationToken cancellationToken);
    Task<List<Review>> GetAllReviews(SieveModel sieveModel, Guid hotelId, CancellationToken cancellationToken);
    Task DeleteReview(Review review, CancellationToken cancellationToken);
    Task<decimal> GetAverageRating(Guid hotelId, CancellationToken cancellationToken);
}
