using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Rooms.Controllers;

[Route("api/rooms")]
[ApiController]
public class RoomsController(IRoomService roomService,
    RoomRequestMapper roomRequestMapper,
    RoomResponseMapper roomResponseMapper) : ControllerBase
{
    /// <summary>
    /// Return list of rooms with pagination, filtering, sorting
    /// </summary>
    /// <param name="sieveModel"></param>
    /// <returns>list of available rooms</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RoomResponse>>> GetRooms([FromQuery] SieveModel sieveModel)
    {
        var rooms = await roomService.GetRooms(sieveModel);
        var roomsResponse = roomResponseMapper.MapRoomListToRoomResponseList(rooms);
        return Ok(roomsResponse);
    }

    /// <summary>
    /// Return room by room id if room id exist
    /// </summary>
    /// <param name="roomId"></param>
    ///  /// <response code="200">If the room exist.</response>
    /// <response code="404">If the room not exist.</response>
    /// <returns>room if exist or not found</returns>
    [HttpGet("{roomId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomResponse>> GetRoom([FromRoute] Guid roomId)
    {
        var room = await roomService.GetRoomById(roomId);
        var roomResponse = roomResponseMapper.MapRoomToRoomResponse(room);
        return Ok(roomResponse);
    }

    /// <summary>
    /// Add new room with valid data
    /// </summary>
    /// <param name="addRoomRequest"></param>
    /// <response code="201">If the room created.</response>
    /// <response code="400">If the room data not valid.</response>
    /// <returns>created room</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomRequest addRoomRequest)
    {
        var room = roomRequestMapper.MapAddRoomRequestToRoom(addRoomRequest);
        await roomService.AddRoom(room);
        
        var roomResponse = roomResponseMapper.MapRoomToRoomResponse(room);
        return CreatedAtAction(nameof(GetRoom),
            new { roomId = room.Id }, roomResponse);
    }
    
    /// <summary>
    /// Partially update the room information
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="roomPatchDocument"></param>
    /// <response code="204">If room updated successfully.</response>
    /// <response code="404">If the room not exist.</response>
    /// <returns>No content if updated successfully or not found.</returns>
    [HttpPatch("{roomId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoom([FromRoute] Guid roomId, JsonPatchDocument<UpdateRoomRequest> roomPatchDocument)
    {
        var room = await roomService.GetRoomById(roomId);

        var updateRoomRequest = roomRequestMapper.MapRoomToUpdateRoomRequest(room);
        roomPatchDocument.ApplyTo(updateRoomRequest);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        roomRequestMapper.MapUpdateRoomRequestToRoom(updateRoomRequest, room);
        
        await roomService.UpdateRoom(room);
        return NoContent();
    }

    /// <summary>
    /// Soft delete room by room id
    /// </summary>
    /// <param name="roomId"></param>
    /// <response code="204">If the room deleted successfully.</response>
    /// <response code="404">If the hotel not exist.</response>
    /// <returns>No content if room deleted successfully or not found.</returns>
    [HttpDelete("{roomId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoom([FromRoute] Guid roomId)
    {
        await roomService.DeleteRoom(roomId);
        return NoContent();
    }
}
