using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IDiscountService
{
    Task AddDiscount(Discount discount);
    Task UpdateDiscount(Discount discount);
    Task DeleteDiscount(Guid discountId);
    Task<Discount> GetDiscountById(Guid discountId);
    Task<List<Discount>> GetDiscounts(SieveModel sieveModel);
}
