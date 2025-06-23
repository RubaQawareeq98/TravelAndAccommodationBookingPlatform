using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IDiscountRepository
{
    Task AddDiscount(Discount discount, CancellationToken cancellationToken);
    Task UpdateDiscount(Discount discount);
    Task DeleteDiscount(Discount discount, CancellationToken cancellationToken);
    Task<decimal> GetDiscountAmountByRoomId(Guid roomCategoryId);
    Task<List<Discount>> GetAllDiscountsByRoom(Guid roomCategoryId, SieveModel sieveModel, CancellationToken cancellationToken);
    Task<Discount?> GetDiscount(Guid roomCategoryId, Guid discountId, CancellationToken cancellationToken);
}
