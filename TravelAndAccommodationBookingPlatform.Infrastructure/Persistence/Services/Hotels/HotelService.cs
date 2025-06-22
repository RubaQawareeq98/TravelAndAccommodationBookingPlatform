using Microsoft.AspNetCore.Http;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Images;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Hotels;

public class HotelService(IHotelRepository hotelRepository,
    IGalleryImageService galleryImageService,
    IImageService imageService) : IHotelService
{
    public async Task AddHotel(Hotel hotel, CancellationToken cancellationToken = default)
    {
        await hotelRepository.AddHotel(hotel, cancellationToken);
    }

    public async Task<Result<Hotel>> UpdateHotel(Hotel hotel, CancellationToken cancellationToken = default)
    {
        var isHotelExists = await hotelRepository.IsHotelExists(hotel.Id, cancellationToken);
        if (!isHotelExists)
        {
            return Result<Hotel>.Failure(HotelError.HotelNotFound(hotel.Id)); 
        }
        await hotelRepository.UpdateHotel(hotel, cancellationToken);
        return Result<Hotel>.Success(hotel);
    }

    public async Task<List<Hotel>> GetHotels(SieveModel sieveModel, CancellationToken cancellationToken = default)
    {
        return await hotelRepository.GetHotels(sieveModel, cancellationToken);
    }

    public async Task<Result<Hotel>> GetHotelById(Guid hotelId, CancellationToken cancellationToken = default)
    {
        var hotel = await hotelRepository.GetHotelById(hotelId, cancellationToken);
        
        return hotel is null ? Result<Hotel>.Failure(HotelError.HotelNotFound(hotelId)) : Result<Hotel>.Success(hotel);
    }

    public async Task<Result<string>> AddHotelGallery(Guid hotelId, IFormFile file,
        CancellationToken cancellationToken = default)
    {
        var hotelResult = await GetHotelById(hotelId, cancellationToken);
        if (hotelResult.IsFailure)
        {
            return Result<string>.Failure(HotelError.HotelNotFound(hotelId));
        }
        
        var hotel = hotelResult.Value;
        var imagePath = await galleryImageService.AddGalleryImage(hotel.Id, file);

        return Result<string>.Success(imagePath);
    }

    public async Task<Result<string>> UpdateHotelThumbnail(Guid hotelId, IFormFile file,
        CancellationToken cancellationToken = default)
    {
        var hotelResult = await GetHotelById(hotelId, cancellationToken);
        if (hotelResult.IsFailure)
        {
            return Result<string>.Failure(HotelError.HotelNotFound(hotelId));
        }
        
        var hotel = hotelResult.Value;
        
        var url = await imageService.UploadImageAsync(file);
        
        hotel.ThumbnailUrl = url;
        await hotelRepository.UpdateHotel(hotel, cancellationToken);
        
        return Result<string>.Success(url);
    }

    public async Task<Result<List<GalleryImage>>> GetHotelGallery(Guid hotelId, CancellationToken cancellationToken = default)
    {
        var hotelResult = await GetHotelById(hotelId, cancellationToken);
        if (hotelResult.IsFailure)
        {
            return Result<List<GalleryImage>>.Failure(HotelError.HotelNotFound(hotelId));
        }
        
        var gallery = await galleryImageService.GetAllImagesByEntityId(hotelId);
        return Result<List<GalleryImage>>.Success(gallery);
    }

    public async Task<List<RoomCategory>> GetTopFeaturedDealsHotels(int listCount, CancellationToken cancellationToken = default)
    {
        return await hotelRepository.GetFeaturedDealsHotels(listCount, cancellationToken);
    }

    public async Task<bool> IsHotelExist(Guid hotelId, CancellationToken cancellationToken = default)
    {
        return await hotelRepository.IsHotelExists(hotelId, cancellationToken);
    }

    public async Task<Result<string>> GetHotelNameById(Guid hotelId, CancellationToken cancellationToken = default)
    {
        var hotel = await hotelRepository.GetHotelById(hotelId, cancellationToken);
        return hotel is null ? Result<string>.Failure(HotelError.HotelNotFound(hotelId)) : Result<string>.Success(hotel.Name);
    }
    
    public async Task<List<RoomCategory>> GetFilteredRooms(SieveModel sieveModel, List<Guid>? amenityIds, CancellationToken cancellationToken = default)
    {
       return await hotelRepository.GetFilteredRoomCategoriesWithHotel(sieveModel, amenityIds, cancellationToken);
    }
}
