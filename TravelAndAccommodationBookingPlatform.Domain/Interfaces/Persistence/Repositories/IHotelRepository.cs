using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IHotelRepository
{
    Task<List<Hotel>> GetHotels(SieveModel sieveModel);
    Task<Hotel?> GetHotelById(Guid hotelId);
    Task AddHotel(Hotel hotel);
    Task UpdateHotel(Hotel hotel);
    Task<bool> IsHotelExists(Guid hotelId);
    Task<List<RoomInfo>> GetFeaturedDealsHotels(int listCount, CancellationToken cancellationToken = default);
}
