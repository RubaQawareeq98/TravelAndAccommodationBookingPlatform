using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IHotelRepository
{
    Task<List<Hotel>> GetHotels(SieveModel sieveModel, CancellationToken cancellationToken);
    Task<Hotel?> GetHotelById(Guid hotelId, CancellationToken cancellationToken);
    Task AddHotel(Hotel hotel, CancellationToken cancellationToken);
    Task UpdateHotel(Hotel hotel, CancellationToken cancellationToken);
    Task<bool> IsHotelExists(Guid hotelId, CancellationToken cancellationToken);
    Task<List<RoomCategory>> GetFeaturedDealsHotels(int listCount, CancellationToken cancellationToken);
}
