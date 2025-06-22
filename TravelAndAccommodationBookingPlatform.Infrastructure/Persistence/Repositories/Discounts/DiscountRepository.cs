using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Persistence;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Discounts;

public class DiscountRepository(HotelBookingManagementDbContext dbContext,
    ISieveProcessor sieveProcessor,
    IUnitOfWork unitOfWork) : IDiscountRepository
{
    public async Task AddDiscount(Discount discount, CancellationToken cancellationToken)
    {
        await dbContext.Discounts.AddAsync(discount, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task UpdateDiscount(Discount discount)
    {
        dbContext.Discounts.Update(discount);
        await unitOfWork.SaveChanges();
    }

    public async Task DeleteDiscount(Discount discount, CancellationToken cancellationToken)
    {
        dbContext.Discounts.Remove(discount);
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task<decimal> GetDiscountAmountByRoomId(Guid roomCategoryId)
    {
        var currentDate = DateTime.UtcNow;
        var discount = await dbContext.Discounts
            .FirstOrDefaultAsync(d => d.RoomCategoryId == roomCategoryId && d.StartDate <= currentDate && d.EndDate >= currentDate);

        return discount?.DiscountPercentage ?? 0;
    }

    public async Task<List<Discount>> GetAllDiscountsByRoom(Guid roomCategoryId, SieveModel sieveModel, CancellationToken cancellationToken)
    {
        var query = dbContext.Discounts
            .Where(d => d.RoomCategoryId == roomCategoryId)
            .AsNoTracking();
        
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Discount?> GetDiscount(Guid roomCategoryId, Guid discountId, CancellationToken cancellationToken)
    {
        return await dbContext.Discounts
            .FirstOrDefaultAsync(d => d.Id == discountId && d.RoomCategoryId == roomCategoryId,
                cancellationToken);
    }

    public async Task<Discount?> GetDiscount(Guid id)
    {
        return await dbContext.Discounts.FirstOrDefaultAsync(o => o.Id == id);
    }
}
