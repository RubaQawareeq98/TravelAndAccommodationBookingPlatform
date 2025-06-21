using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.Api.RoomCategories.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.RoomCategories.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Api.RoomCategories.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.RoomCategories.Controllers;

[Route("api/roomCategories")]
[ApiController]
public class RoomCategoriesController(IRoomCategoryService roomCategoriesService,
    RoomCategoryRequestMapper roomCategoryRequestMapper,
    RoomCategoryResponseMapper roomCategoryResponseMapper) : ControllerBase
{
    /// <summary>
    /// Return list of roomCategories with pagination, filtering, sorting
    /// </summary>
    /// <returns>list of available roomCategories</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RoomCategoryResponse>>> GetRoomCategories()
    {
        var roomCategories = await roomCategoriesService.GetRoomCategories();
        var roomCategoriesResponse = roomCategoryResponseMapper.MapRoomCategoryListToRoomCategoryResponseList(roomCategories);
        return Ok(roomCategoriesResponse);
    }

    /// <summary>
    /// Return roomCategory by roomCategory id if roomCategory id exist
    /// </summary>
    /// <param name="roomCategoryId"></param>
    ///  /// <response code="200">If the roomCategory exist.</response>
    /// <response code="404">If the roomCategory not exist.</response>
    /// <returns>roomCategory if exist or not found</returns>
    [HttpGet("{roomCategoryId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomCategoryResponse>> GetRoomCategory([FromRoute] Guid roomCategoryId)
    {
        var roomCategory = await roomCategoriesService.GetRoomCategoryById(roomCategoryId);
        var roomCategoryResponse = roomCategoryResponseMapper.MapRoomCategoryToRoomCategoryResponse(roomCategory);
        return Ok(roomCategoryResponse);
    }

    /// <summary>
    /// Add new roomCategory with valid data
    /// </summary>
    /// <param name="addRoomCategoryRequest"></param>
    /// <response code="201">If the roomCategory created.</response>
    /// <response code="400">If the roomCategory data not valid.</response>
    /// <response code="404">If one of the amenities ID not found.</response>
    /// <returns>created roomCategory</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddRoomCategory([FromBody] AddRoomCategoryRequest addRoomCategoryRequest)
    {
        var roomCategory = roomCategoryRequestMapper.MapAddRoomCategoryRequestToRoomCategory(addRoomCategoryRequest);
        var amenitiesIds = addRoomCategoryRequest.AmenitiesIds ?? [];
        
        await roomCategoriesService.AddRoomCategory(roomCategory, amenitiesIds);
        
        var roomCategoryResponse = roomCategoryResponseMapper.MapRoomCategoryToRoomCategoryResponse(roomCategory);
        return CreatedAtAction(nameof(GetRoomCategory),
            new { roomCategoryId = roomCategory.Id }, roomCategoryResponse);
    }
    
    /// <summary>
    /// Partially update the roomCategory information
    /// </summary>
    /// <param name="roomCategoryId"></param>
    /// <param name="roomCategoryPatchDocument"></param>
    /// <response code="204">If roomCategory updated successfully.</response>
    /// <response code="404">If the roomCategory not exist.</response>
    /// <returns>No content if updated successfully or not found.</returns>
    [HttpPatch("{roomCategoryId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoomCategory([FromRoute] Guid roomCategoryId, JsonPatchDocument<UpdateRoomCategoryRequest> roomCategoryPatchDocument)
    {
        var roomCategory = await roomCategoriesService.GetRoomCategoryById(roomCategoryId);

        var updateRoomCategoryRequest = roomCategoryRequestMapper.MapRoomCategoryToUpdateRoomCategoryRequest(roomCategory);
        roomCategoryPatchDocument.ApplyTo(updateRoomCategoryRequest);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        roomCategoryRequestMapper.MapUpdateRoomCategoryRequestToRoomCategory(updateRoomCategoryRequest, roomCategory);
        
        await roomCategoriesService.UpdateRoomCategory(roomCategory);
        return NoContent();
    }

    /// <summary>
    /// Soft delete roomCategory by roomCategory id
    /// </summary>
    /// <param name="roomCategoryId"></param>
    /// <response code="204">If the roomCategory deleted successfully.</response>
    /// <response code="404">If the hotel not exist.</response>
    /// <returns>No content if roomCategory deleted successfully or not found.</returns>
    [HttpDelete("{roomCategoryId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoomCategory([FromRoute] Guid roomCategoryId)
    {
        await roomCategoriesService.DeleteRoomCategory(roomCategoryId);
        return NoContent();
    }

    /// <summary>
    /// Search for a room by different search criteria & by amenities
    /// </summary>
    /// <param name="searchRequest"></param>
    /// <returns></returns>
    [HttpGet("search")]
    public async Task<IActionResult> GetFilteredRooms([FromQuery] RoomSearchRequest searchRequest)
    {
        var sieveModel = roomCategoryRequestMapper.MapSearchCritereaToSieveModel(searchRequest);
        var rooms = await roomCategoriesService.GetFilteredRooms(sieveModel, searchRequest.AmenitiesIds);

        var roomCategories = roomCategoryResponseMapper.MapRoomCategoryListToRoomCategoryResponseList(rooms);
        return Ok(roomCategories);
    }
}
