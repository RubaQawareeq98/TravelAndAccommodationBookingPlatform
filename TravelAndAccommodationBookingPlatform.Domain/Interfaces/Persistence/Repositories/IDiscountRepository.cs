using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IDiscountRepository
{
    Task AddDiscount(Discount discount);
    Task UpdateDiscount(Discount discount);
    Task<Discount?> GetDiscount(Guid id);
    Task<List<Discount>> GetAllDiscounts(SieveModel sieveModel);
    Task DeleteDiscount(Discount discount);
}
