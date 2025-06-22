using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Extensions;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Rooms.Controllers;

[Route("api/hotels/{hotelId:guid}/room-categories/{roomCategoryId:guid}/rooms")]
[ApiController]
public class RoomsController(
    IRoomService roomService,
    RoomRequestMapper roomRequestMapper,
    RoomResponseMapper roomResponseMapper) : ControllerBase
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoom(
        [FromRoute] Guid hotelId,
        [FromRoute] Guid roomCategoryId,
        [FromRoute] Guid roomId,
        CancellationToken cancellationToken)
    {
        var result = await roomService.DeleteRoom(hotelId, roomCategoryId, roomId, cancellationToken);
        return result.ToActionResult();
    }
}
