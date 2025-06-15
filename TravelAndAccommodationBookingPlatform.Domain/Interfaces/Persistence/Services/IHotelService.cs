using Microsoft.AspNetCore.Http;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IHotelService
{
    Task AddHotel(Hotel hotel);
    Task UpdateHotel(Hotel hotel);
    Task<List<Hotel>> GetHotels(SieveModel sieveModel);
    Task<Hotel> GetHotelById(Guid hotelId);
    Task<string> AddHotelGallery(Guid hotelId, IFormFile file);
    Task<string> UpdateHotelThumbnail(Guid hotelId, IFormFile file);
    Task<List<GalleryImage>> GetHotelGallery(Guid hotelId);
}
