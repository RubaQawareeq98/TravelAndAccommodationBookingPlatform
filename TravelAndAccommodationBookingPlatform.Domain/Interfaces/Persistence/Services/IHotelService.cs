using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IHotelService
{
    Task AddHotelAsync(Hotel hotel);
    Task UpdateHotelAsync(Hotel hotel);
    Task<List<Hotel>> GetHotelsAsync(SieveModel sieveModel);
    Task<Hotel?> GetHotelByIdAsync(Guid hotelId);
}
