using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Persistence;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Reviews;

public class ReviewRepository(HotelBookingManagementDbContext dbContext,
    ISieveProcessor sieveProcessor,
    IUnitOfWork unitOfWork) : IReviewRepository
{
    public async Task AddReview(Review review, CancellationToken cancellationToken)
    {
        await dbContext.Reviews.AddAsync(review, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task UpdateReview(Review review, CancellationToken cancellationToken)
    {
        dbContext.Reviews.Update(review);
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task DeleteReview(Review review, CancellationToken cancellationToken)
    {
        dbContext.Reviews.Remove(review);
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task<decimal> GetAverageRating(Guid hotelId, CancellationToken cancellationToken)
    {
        return (decimal)await dbContext.Reviews
            .Where(r => r.HotelId == hotelId)
            .AverageAsync(r => r.Rating, cancellationToken: cancellationToken);
    }

    public async Task<Review?> GetReview(Guid hotelId, Guid reviewId, CancellationToken cancellationToken)
    {
        return await dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId && r.HotelId == hotelId, cancellationToken: cancellationToken);
    }

    public async Task<List<Review>> GetAllReviews(SieveModel sieveModel, Guid hotelId, CancellationToken cancellationToken)
    {
        var query = dbContext.Reviews
            .Where(r => r.HotelId == hotelId)
            .AsNoTracking();
        
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync(cancellationToken: cancellationToken);
    }
}
