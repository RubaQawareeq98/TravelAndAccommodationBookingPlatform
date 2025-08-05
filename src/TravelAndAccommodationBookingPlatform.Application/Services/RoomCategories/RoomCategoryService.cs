using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Application.Services.RoomCategories;

public class RoomCategoryService(IRoomCategoryRepository roomCategoryRepository,
    IAmenityService amenityService,
    IHotelService hotelService) : IRoomCategoryService
{
    public async Task<Result<RoomCategory>> AddRoomCategory(Guid hotelId, RoomCategory roomCategory, List<Guid> amenitiesIds,
        CancellationToken cancellationToken = default)
    {
        foreach (var id in amenitiesIds)
        {
            var result = await amenityService.GetAmenityById(id, cancellationToken);
            if (result.IsFailure)
            {
                return Result<RoomCategory>.Failure(AmenityError.AmenityNotFound(id));
            }
            
            var amenity = result.Value;
            roomCategory.Amenities.Add(amenity);
        }
        
        var isHotelExist = await hotelService.IsHotelExist(hotelId, cancellationToken);
        if (!isHotelExist)
        {
            return Result<RoomCategory>.Failure(HotelError.HotelNotFound(hotelId));
        }

        roomCategory.HotelId = hotelId;
        await roomCategoryRepository.AddRoomCategory(roomCategory, cancellationToken);
        
        return Result<RoomCategory>.Success(roomCategory);
    }

    public async Task<Result> UpdateRoomCategory(Guid hotelId, RoomCategory roomCategory,
        CancellationToken cancellationToken = default)
    {
        var isHotelExist = await hotelService.IsHotelExist(hotelId, cancellationToken);
        if (!isHotelExist)
        {
            return Result.Failure(HotelError.HotelNotFound(hotelId));
        }
        
        await roomCategoryRepository.UpdateRoomCategory(roomCategory, cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteRoomCategory(Guid hotelId, Guid roomCategoryId, CancellationToken cancellationToken = default)
    {
        var isHotelExist = await hotelService.IsHotelExist(hotelId, cancellationToken);
        if (!isHotelExist)
        {
            return Result.Failure(HotelError.HotelNotFound(hotelId));
        }
        
        var result = await GetRoomCategoryById(hotelId, roomCategoryId, cancellationToken);
        if (result.IsFailure)
        {
            return Result.Failure(RoomCategoryError.RoomCategoryNotFound(roomCategoryId));
        }
        
        var roomCategory = result.Value;
        await roomCategoryRepository.DeleteRoomCategory(roomCategory, cancellationToken);
        
        return Result<RoomCategory>.Success(roomCategory);
    }

    public async Task<Result<RoomCategory>> GetRoomCategoryById(Guid hotelId, Guid roomCategoryId,
        CancellationToken cancellationToken = default)
    {
        var isHotelExist = await hotelService.IsHotelExist(hotelId, cancellationToken);
        if (!isHotelExist)
        {
            return Result<RoomCategory>.Failure(HotelError.HotelNotFound(hotelId));
        }
        
        var roomCategory = await roomCategoryRepository.GetRoomCategoryById(roomCategoryId, cancellationToken);
        return roomCategory is null ? Result<RoomCategory>.Failure(RoomCategoryError.RoomCategoryNotFound(roomCategoryId)) : Result<RoomCategory>.Success(roomCategory);
    }

    public async Task<Result<List<RoomCategory>>> GetRoomCategories(Guid hotelId,
        SieveModel sieveModel,
        CancellationToken cancellationToken = default)
    {
        var isHotelExist = await hotelService.IsHotelExist(hotelId, cancellationToken);
        if (!isHotelExist)
        {
            return Result<List<RoomCategory>>.Failure(HotelError.HotelNotFound(hotelId));
        }
        
        var roomCategories = await roomCategoryRepository.GetAllRoomCategoriesByHotelId(hotelId, sieveModel, cancellationToken);
        return Result<List<RoomCategory>>.Success(roomCategories);
    }
}
