using Microsoft.AspNetCore.Http;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IHotelService
{
    Task<Result<Hotel>> AddHotel(Hotel hotel, CancellationToken cancellationToken = default);
    Task<Result<Hotel>> UpdateHotel(Hotel hotel, CancellationToken cancellationToken = default);
    Task<List<Hotel>> GetHotels(SieveModel sieveModel, CancellationToken cancellationToken = default);
    Task<Result<Hotel>> GetHotelById(Guid hotelId, CancellationToken cancellationToken = default);
    Task<Result<string>> AddHotelGallery(Guid hotelId, IFormFile file, CancellationToken cancellationToken = default);
    Task<Result<string>> UpdateHotelThumbnail(Guid hotelId, IFormFile file,
        CancellationToken cancellationToken = default);
    Task<Result<List<GalleryImage>>> GetHotelGallery(Guid hotelId, CancellationToken cancellationToken = default);
    Task<List<RoomCategory>> GetTopFeaturedDealsHotels(int listCount, CancellationToken cancellationToken = default);
    Task<bool> IsHotelExist(Guid hotelId, CancellationToken cancellationToken = default);
    Task<List<RoomCategory>> GetFilteredRooms(SieveModel sieveModel, List<Guid>? amenityIds,
        CancellationToken cancellationToken = default);
}
