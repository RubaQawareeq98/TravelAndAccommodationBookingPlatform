using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Extensions;
using TravelAndAccommodationBookingPlatform.Api.Images.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Images.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Rooms.Controllers;

[Route("api/hotels/{hotelId:guid}/room-categories/{roomCategoryId:guid}/rooms")]
[Authorize]
[ApiController]
public class RoomsController(
    IRoomService roomService,
    RoomRequestMapper roomRequestMapper,
    RoomResponseMapper roomResponseMapper,
    GalleryImageMapper galleryImageMapper) : ControllerBase
{
    /// <summary>
    /// Retrieves a paginated, filtered, and sorted list of rooms for a given hotel and room category.
    /// </summary>
    /// <param name="hotelId">Hotel ID.</param>
    /// <param name="roomCategoryId">Room Category ID.</param>
    /// <param name="sieveModel">Sieve model for filtering, sorting, and pagination.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of rooms.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRooms(
        [FromRoute] Guid hotelId,
        [FromRoute] Guid roomCategoryId,
        [FromQuery] SieveModel sieveModel,
        CancellationToken cancellationToken)
    {
        var result = await roomService.GetRooms(hotelId, roomCategoryId, sieveModel, cancellationToken);
        return result.Map(roomResponseMapper.MapRoomListToRoomResponseList).ToActionResult();
    }

    /// <summary>
    /// Retrieves a specific room by ID.
    /// </summary>
    /// <param name="hotelId">Hotel ID.</param>
    /// <param name="roomCategoryId">Room Category ID.</param>
    /// <param name="roomId">Room ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Room details or 404 if not found.</returns>
    [HttpGet("{roomId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRoom(
        [FromRoute] Guid hotelId,
        [FromRoute] Guid roomCategoryId,
        [FromRoute] Guid roomId,
        CancellationToken cancellationToken)
    {
        var result = await roomService.GetRoomById(hotelId, roomCategoryId, roomId, cancellationToken);
        return result.Map(roomResponseMapper.MapRoomToRoomResponse).ToActionResult();
    }

    /// <summary>
    /// Creates a new room in the specified hotel and room category.
    /// </summary>
    /// <param name="hotelId">Hotel ID.</param>
    /// <param name="roomCategoryId">Room Category ID.</param>
    /// <param name="addRoomRequest">Room creation data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created room details with location header.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddRoom(
        [FromRoute] Guid hotelId,
        [FromRoute] Guid roomCategoryId,
        [FromBody] AddRoomRequest addRoomRequest,
        CancellationToken cancellationToken)
    {
        var room = roomRequestMapper.MapAddRoomRequestToRoom(addRoomRequest);
        var result = await roomService.AddRoom(room, hotelId, roomCategoryId, cancellationToken);

        return result.ToActionResult(addedRoom =>
        {
            var roomResponse = roomResponseMapper.MapRoomToRoomResponse(addedRoom);
            return CreatedAtAction(nameof(GetRoom),
                new { hotelId, roomCategoryId, roomId = room.Id },
                roomResponse);
        });
    }

    /// <summary>
    /// Partially updates the room information using a JSON Patch document.
    /// </summary>
    /// <param name="hotelId">Hotel ID.</param>
    /// <param name="roomCategoryId">Room Category ID.</param>
    /// <param name="roomId">Room ID.</param>
    /// <param name="roomPatchDocument">Patch document with updated room fields.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content if updated successfully; otherwise, 404 or 400.</returns>
    [HttpPatch("{roomId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateRoom(
        [FromRoute] Guid hotelId,
        [FromRoute] Guid roomCategoryId,
        [FromRoute] Guid roomId,
        [FromBody] JsonPatchDocument<UpdateRoomRequest> roomPatchDocument,
        CancellationToken cancellationToken)
    {
        var result = await roomService.GetRoomById(hotelId, roomCategoryId, roomId, cancellationToken);
        if (result.IsFailure)
        {
            return result.ToActionResult();
        }

        var room = result.Value;
        var updateRoomRequest = roomRequestMapper.MapRoomToUpdateRoomRequest(room);
        roomPatchDocument.ApplyTo(updateRoomRequest, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var isValid = TryValidateModel(updateRoomRequest);
        if (!isValid)
        {
            return ValidationProblem(ModelState);
        }

        roomRequestMapper.MapUpdateRoomRequestToRoom(updateRoomRequest, room);
        await roomService.UpdateRoom(room);
        return NoContent();
    }

    /// <summary>
    /// Soft-deletes a room by ID.
    /// </summary>
    /// <param name="hotelId">Hotel ID.</param>
    /// <param name="roomCategoryId">Room Category ID.</param>
    /// <param name="roomId">Room ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content if deleted successfully; otherwise, 404.</returns>
    [HttpDelete("{roomId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteRoom(
        [FromRoute] Guid hotelId,
        [FromRoute] Guid roomCategoryId,
        [FromRoute] Guid roomId,
        CancellationToken cancellationToken)
    {
        var result = await roomService.DeleteRoom(hotelId, roomCategoryId, roomId, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Uploads a gallery image for a room.
    /// </summary>
    /// <param name="roomCategoryId"></param>
    /// <param name="roomId">The ID of the room.</param>
    /// <param name="imageUploadRequest">The image file to be uploaded.</param>
    /// <param name="hotelId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The URL of the uploaded image.</returns>
    [HttpPost("{roomId:guid}/gallery")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddImageGalleryToHotel([FromRoute] Guid hotelId,
        [FromRoute] Guid roomCategoryId,
        [FromRoute] Guid roomId,
        [FromForm] ImageUploadRequest imageUploadRequest,
        CancellationToken cancellationToken)
    {
        var result = await roomService.AddRoomGallery(hotelId, roomCategoryId, roomId, imageUploadRequest.File, cancellationToken);
        return result.IsFailure ? result.ToActionResult() : Ok(new { imageUrl = result.Value });
    }

    /// <summary>
    /// Return list of gallery image for a room
    /// </summary>
    /// <param name="roomCategoryId"></param>
    /// <param name="roomId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="hotelId"></param>
    /// <returns>The room gallery</returns>
    [HttpGet("{roomId:guid}/gallery")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetHotelGallery([FromRoute]Guid hotelId,
        [FromRoute] Guid roomCategoryId,
        [FromRoute] Guid roomId,
        CancellationToken cancellationToken)
    {
        var result = await roomService.GetRoomGallery(hotelId, roomCategoryId, roomId, cancellationToken);
        return result.Map(galleryImageMapper.MapGalleryImageToResponse).ToActionResult();
    }
}
