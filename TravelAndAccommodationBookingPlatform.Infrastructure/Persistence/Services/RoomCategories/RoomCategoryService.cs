using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.RoomCategories;

public class RoomCategorieservice(IRoomCategoryRepository roomCategoryRepository,
    IAmenityService amenityService,
    IHotelService hotelService) : IRoomCategoryService
{
    public async Task AddRoomCategory(RoomCategory roomCategory, List<Guid> amenitiesIds)
    {
        foreach (var id in amenitiesIds)
        {
            var amenity = await amenityService.GetAmenityById(id);
            if (amenity is null)
            {
                throw new NotFoundException($"No amenity with id {id} could be found.");
            }
         //   roomCategory.Amenities.Add(amenity);
        }
        
        var isHotelExist = await hotelService.IsHotelExist(roomCategory.HotelId);
        if (!isHotelExist)
        {
            throw new NotFoundException($"No hotel with id {roomCategory.HotelId} could be found.");
        }
        
        await roomCategoryRepository.AddRoomCategory(roomCategory);
    }

    public async Task UpdateRoomCategory(RoomCategory roomCategory)
    {
        await roomCategoryRepository.UpdateRoomCategory(roomCategory);
    }

    public async Task DeleteRoomCategory(Guid roomCategoryId)
    {
        var roomCategory = await GetRoomCategoryById(roomCategoryId);
        await roomCategoryRepository.DeleteRoomCategory(roomCategory);
    }

    public async Task<RoomCategory> GetRoomCategoryById(Guid roomCategoryId)
    {
        var roomCategory = await roomCategoryRepository.GetRoomCategory(roomCategoryId);
        if (roomCategory is null)
        {
            throw new NotFoundException($"RoomCategory with if {roomCategoryId} not found");
        }
        
        return roomCategory;
    }

    public async Task<List<RoomCategory>> GetRoomCategories()
    {
        return await roomCategoryRepository.GetAllRoomCategories();
    }

    public async Task<List<RoomCategory>> GetFilteredRooms(SieveModel sieveModel, List<Guid>? amenityIds)
    {
        return await roomCategoryRepository.GetFilteredRoomCategories(sieveModel, amenityIds);
    }
}
