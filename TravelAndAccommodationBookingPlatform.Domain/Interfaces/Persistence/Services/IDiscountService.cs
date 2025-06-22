using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IDiscountService
{
    Task UpdateDiscount(Discount discount);
    Task<Result<List<Discount>>> GetDiscountsByRoom(Guid hotelId, Guid roomCategoryId, SieveModel sieveModel,
        CancellationToken cancellationToken = default);
    Task<Result<Discount>> GetDiscountById(Guid hotelId, Guid roomCategoryId, Guid discountId,
        CancellationToken cancellationToken);

    Task<Result<Discount>> AddDiscount(Guid hotelId, Guid roomCategoryId, Discount discount,
        CancellationToken cancellationToken);

    Task<Result> DeleteDiscount(Guid hotelId, Guid roomCategoryId, Guid discountId, CancellationToken cancellationToken);
}
