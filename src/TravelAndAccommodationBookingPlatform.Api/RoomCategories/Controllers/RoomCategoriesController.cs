using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Extensions;
using TravelAndAccommodationBookingPlatform.Api.RoomCategories.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.RoomCategories.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.RoomCategories.Controllers;

[Route("api/hotels/{hotelId:guid}/room-categories")]
[Authorize]
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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRoomCategories([FromRoute] Guid hotelId,
        [FromQuery] SieveModel sieveModel,
        CancellationToken cancellationToken)
    {
        var result = await roomCategoriesService.GetRoomCategories(hotelId, sieveModel, cancellationToken);
        return result.Map(roomCategoryResponseMapper.MapRoomCategoryListToRoomCategoryResponseList).ToActionResult();
    }

    /// <summary>
    /// Return roomCategory by roomCategory id if roomCategory id exist
    /// </summary>
    /// <param name="roomCategoryId"></param>
    /// <param name="hotelId"></param>
    /// <response code="200">If the roomCategory exist.</response>
    /// <response code="404">If the roomCategory not exist.</response>
    /// <returns>roomCategory if exist or not found</returns>
    [HttpGet("{roomCategoryId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRoomCategory([FromRoute] Guid roomCategoryId, Guid hotelId)
    {
        var result = await roomCategoriesService.GetRoomCategoryById(hotelId, roomCategoryId);
        return result.Map(roomCategoryResponseMapper.MapRoomCategoryToRoomCategoryResponse).ToActionResult();
    }

    /// <summary>
    /// Add new roomCategory with valid data
    /// </summary>
    /// <param name="hotelId"></param>
    /// <param name="addRoomCategoryRequest"></param>
    /// <response code="201">If the roomCategory created.</response>
    /// <response code="400">If the roomCategory data not valid.</response>
    /// <response code="404">If one of the amenities ID not found.</response>
    /// <returns>created roomCategory</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddRoomCategory(Guid hotelId, [FromBody] AddRoomCategoryRequest addRoomCategoryRequest)
    {
        var roomCategory = roomCategoryRequestMapper.MapAddRoomCategoryRequestToRoomCategory(addRoomCategoryRequest);
        var amenitiesIds = addRoomCategoryRequest.AmenitiesIds ?? [];
        
        var result = await roomCategoriesService.AddRoomCategory(hotelId, roomCategory, amenitiesIds);
        
        return result.ToActionResult(addedRoom =>
        {
            var roomCategoryResponse = roomCategoryResponseMapper.MapRoomCategoryToRoomCategoryResponse(addedRoom);
            return CreatedAtAction(nameof(GetRoomCategory), new { roomCategoryId = roomCategory.Id, hotelId }, roomCategoryResponse);
        });
    }

    /// <summary>
    /// Partially update the roomCategory information
    /// </summary>
    /// <param name="roomCategoryId"></param>
    /// <param name="roomCategoryPatchDocument"></param>
    /// <param name="hotelId"></param>
    /// <response code="204">If roomCategory updated successfully.</response>
    /// <response code="404">If the roomCategory not exist.</response>
    /// <returns>No content if updated successfully or not found.</returns>
    [HttpPatch("{roomCategoryId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateRoomCategory([FromRoute] Guid roomCategoryId, JsonPatchDocument<UpdateRoomCategoryRequest> roomCategoryPatchDocument, Guid hotelId)
    {
        var result = await roomCategoriesService.GetRoomCategoryById(hotelId, roomCategoryId);
        if (result.IsFailure)
        {
            return result.ToActionResult();
        }

        var roomCategory = result.Value;
        var updateRoomCategoryRequest = roomCategoryRequestMapper.MapRoomCategoryToUpdateRoomCategoryRequest(roomCategory);
        roomCategoryPatchDocument.ApplyTo(updateRoomCategoryRequest);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        roomCategoryRequestMapper.MapUpdateRoomCategoryRequestToRoomCategory(updateRoomCategoryRequest, roomCategory);
        
        var updateResult = await roomCategoriesService.UpdateRoomCategory(hotelId, roomCategory);
        return updateResult.ToActionResult();
    }

    /// <summary>
    /// Soft delete roomCategory by roomCategory id
    /// </summary>
    /// <param name="roomCategoryId"></param>
    /// <param name="hotelId"></param>
    /// <response code="204">If the roomCategory deleted successfully.</response>
    /// <response code="404">If the hotel not exist.</response>
    /// <returns>No content if roomCategory deleted successfully or not found.</returns>
    [HttpDelete("{roomCategoryId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteRoomCategory([FromRoute] Guid roomCategoryId, Guid hotelId)
    {
        var result = await roomCategoriesService.DeleteRoomCategory(hotelId, roomCategoryId);
        return result.ToActionResult();
    }
}
