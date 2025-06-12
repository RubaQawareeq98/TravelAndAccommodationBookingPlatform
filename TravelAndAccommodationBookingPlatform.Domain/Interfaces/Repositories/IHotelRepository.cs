using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

public interface IHotelRepository
{
    Task<List<Hotel?>> GetHotels(SieveModel sieveModel);
    Task<Hotel?> GetHotelById(Guid hotelId);
    Task AddHotel(Hotel hotel);
    Task UpdateHotel(Hotel hotel);
}
