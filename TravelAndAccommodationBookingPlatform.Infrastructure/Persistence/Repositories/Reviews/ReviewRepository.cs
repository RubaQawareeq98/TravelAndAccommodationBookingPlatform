using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Reviews;

public class ReviewRepository(HotelBookingManagementDbContext dbContext, ISieveProcessor sieveProcessor) : IReviewRepository
{
    public async Task AddReview(Review review)
    {
        await dbContext.Reviews.AddAsync(review);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateReview(Review review)
    {
        dbContext.Reviews.Update(review);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteReview(Review review)
    {
        dbContext.Reviews.Remove(review);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Review?> GetReview(Guid id)
    {
        return await dbContext.Reviews.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Review>> GetAllReviews(SieveModel sieveModel)
    {
        var query = dbContext.Reviews.AsQueryable();
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync();
    }
}
