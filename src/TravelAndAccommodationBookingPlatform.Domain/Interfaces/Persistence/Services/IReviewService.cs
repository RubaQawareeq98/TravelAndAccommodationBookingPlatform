using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IReviewService
{
    Task<Result<Review>> AddReview(Guid hotelId, Review review, CancellationToken cancellationToken = default);
    Task UpdateReview(Review review, CancellationToken cancellationToken = default);
    Task<Result> DeleteReview(Guid hotelId, Guid reviewId, CancellationToken cancellationToken = default);
    Task<Result<Review>> GetReviewById(Guid hotelId, Guid reviewId, CancellationToken cancellationToken = default);
    Task<Result<List<Review>>> GetReviews(SieveModel sieveModel, Guid hotelId, CancellationToken cancellationToken = default);
    Task<Result<decimal>> CalculateHotelAverageRating(Guid hotelId, CancellationToken cancellationToken = default);
}
