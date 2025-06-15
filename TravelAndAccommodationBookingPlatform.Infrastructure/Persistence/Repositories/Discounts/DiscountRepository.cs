using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Discounts;

public class DiscountRepository(HotelBookingManagementDbContext dbContext, ISieveProcessor sieveProcessor) : IDiscountRepository
{
    public async Task AddDiscount(Discount discount)
    {
        await dbContext.Discounts.AddAsync(discount);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateDiscount(Discount discount)
    {
        dbContext.Discounts.Update(discount);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteDiscount(Discount discount)
    {
        dbContext.Discounts.Remove(discount);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Discount?> GetDiscount(Guid id)
    {
        return await dbContext.Discounts.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Discount>> GetAllDiscounts(SieveModel sieveModel)
    {
        var query = dbContext.Discounts.AsQueryable();
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync();
    }
}
