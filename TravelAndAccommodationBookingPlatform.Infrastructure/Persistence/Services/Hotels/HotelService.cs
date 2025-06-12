using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Hotels;

public class HotelService(IHotelRepository hotelRepository) : IHotelService
{
    public async Task AddHotelAsync(Hotel hotel)
    {
        await hotelRepository.AddHotel(hotel);
    }

    public async Task UpdateHotelAsync(Hotel hotel)
    {
        var isHotelExists = await hotelRepository.IsHotelExists(hotel.Id);
        if (!isHotelExists)
        {
            throw new NotFoundException($"Hotel with this id {hotel.Id} does not exist.");
        }
        await hotelRepository.UpdateHotel(hotel);
    }

    public async Task<List<Hotel>> GetHotelsAsync(SieveModel sieveModel)
    {
        return await hotelRepository.GetHotels(sieveModel);
    }

    public async Task<Hotel?> GetHotelByIdAsync(Guid hotelId)
    {
        return await hotelRepository.GetHotelById(hotelId);
    }
}
