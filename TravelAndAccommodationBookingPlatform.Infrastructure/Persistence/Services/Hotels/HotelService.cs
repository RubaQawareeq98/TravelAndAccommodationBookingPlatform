using Microsoft.AspNetCore.Http;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Hotels;

public class HotelService(IHotelRepository hotelRepository,
    IGalleryImageService galleryImageService,
    IImageService imageService) : IHotelService
{
    public async Task AddHotel(Hotel hotel)
    {
        await hotelRepository.AddHotel(hotel);
    }

    public async Task UpdateHotel(Hotel hotel)
    {
        var isHotelExists = await hotelRepository.IsHotelExists(hotel.Id);
        if (!isHotelExists)
        {
            throw new NotFoundException($"Hotel with this id {hotel.Id} does not exist.");
        }
        await hotelRepository.UpdateHotel(hotel);
    }

    public async Task<List<Hotel>> GetHotels(SieveModel sieveModel)
    {
        return await hotelRepository.GetHotels(sieveModel);
    }

    public async Task<Hotel> GetHotelById(Guid hotelId)
    {
        var hotel = await hotelRepository.GetHotelById(hotelId);
        if (hotel is null)
        {
            throw new NotFoundException($"Hotel with this id {hotelId} does not exist.");
        }
        return hotel;
    }

    public async Task<string> AddHotelGallery(Guid hotelId, IFormFile file)
    {
        var hotel = await GetHotelById(hotelId);

        var imagePath = await galleryImageService.AddGalleryImage(hotel.Id, file);
        return imagePath;
    }

    public async Task<string> UpdateHotelThumbnail(Guid hotelId, IFormFile file)
    {
        var hotel = await GetHotelById(hotelId);
        
        var url = await imageService.UploadImageAsync(file);
        
        hotel.ThumbnailUrl = url;
        await hotelRepository.UpdateHotel(hotel);
        
        return url;
    }

    public async Task<List<GalleryImage>> GetHotelGallery(Guid hotelId)
    {
        var hotel = await GetHotelById(hotelId);
        
        var gallery = await galleryImageService.GetAllImagesByEntityId(hotel.Id);
        return gallery;
    }

    public async Task<List<RoomInfo>> GetTopFeaturedDealsHotels(int listCount, CancellationToken cancellationToken = default)
    {
        return await hotelRepository.GetFeaturedDealsHotels(listCount, cancellationToken);
    }
}
