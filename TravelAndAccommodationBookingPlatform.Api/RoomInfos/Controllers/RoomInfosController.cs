using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.RoomInfos.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.RoomInfos.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.RoomInfos.Controllers;

[Route("api/roomInfos")]
[ApiController]
public class RoomInfosController(IRoomInfoService roomInfoService, RoomInfoRequestMapper roomInfoRequestMapper) : ControllerBase
{
    /// <summary>
    /// Return list of roomInfos with pagination, filtering, sorting
    /// </summary>
    /// <param name="sieveModel"></param>
    /// <returns>list of available roomInfos</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RoomInfo>>> GetRoomInfos([FromQuery] SieveModel sieveModel)
    {
        var roomInfos = await roomInfoService.GetRoomInfosAsync(sieveModel);
        return Ok(roomInfos);
    }

    /// <summary>
    /// Return roomInfo by roomInfo id if roomInfo id exist
    /// </summary>
    /// <param name="roomInfoId"></param>
    ///  /// <response code="200">If the roomInfo exist.</response>
    /// <response code="404">If the roomInfo not exist.</response>
    /// <returns>roomInfo if exist or not found</returns>
    [HttpGet("{roomInfoId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomInfo>> GetRoomInfo([FromRoute] Guid roomInfoId)
    {
        var roomInfo = await roomInfoService.GetRoomInfoByIdAsync(roomInfoId);
        return Ok(roomInfo);
    }

    /// <summary>
    /// Add new roomInfo with valid data
    /// </summary>
    /// <param name="addRoomInfoRequest"></param>
    /// <response code="201">If the roomInfo created.</response>
    /// <response code="400">If the roomInfo data not valid.</response>
    /// <returns>created roomInfo</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddRoomInfo([FromBody] AddRoomInfoRequest addRoomInfoRequest)
    {
        var roomInfo = roomInfoRequestMapper.MapAddRoomInfoRequestToRoomInfo(addRoomInfoRequest);
        await roomInfoService.AddRoomInfoAsync(roomInfo);
        
        return CreatedAtAction(nameof(GetRoomInfo),
            new { roomInfoId = roomInfo.Id }, roomInfo);
    }
    
    /// <summary>
    /// Partially update the roomInfo information
    /// </summary>
    /// <param name="roomInfoId"></param>
    /// <param name="roomInfoPatchDocument"></param>
    /// <response code="204">If roomInfo updated successfully.</response>
    /// <response code="404">If the roomInfo not exist.</response>
    /// <returns>No content if updated successfully or not found.</returns>
    [HttpPatch("{roomInfoId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoomInfo([FromRoute] Guid roomInfoId, JsonPatchDocument<UpdateRoomInfoRequest> roomInfoPatchDocument)
    {
        var roomInfo = await roomInfoService.GetRoomInfoByIdAsync(roomInfoId);

        var updateRoomInfoRequest = roomInfoRequestMapper.MapRoomInfoToUpdateRoomInfoRequest(roomInfo);
        roomInfoPatchDocument.ApplyTo(updateRoomInfoRequest);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        roomInfoRequestMapper.MapUpdateRoomInfoRequestToRoomInfo(updateRoomInfoRequest, roomInfo);
        
        await roomInfoService.UpdateRoomInfoAsync(roomInfo);
        return NoContent();
    }

    /// <summary>
    /// Soft delete roomInfo by roomInfo id
    /// </summary>
    /// <param name="roomInfoId"></param>
    /// <response code="204">If the roomInfo deleted successfully.</response>
    /// <response code="404">If the hotel not exist.</response>
    /// <returns>No content if roomInfo deleted successfully or not found.</returns>
    [HttpDelete("{roomInfoId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoomInfo([FromRoute] Guid roomInfoId)
    {
        await roomInfoService.DeleteRoomInfoAsync(roomInfoId);
        return NoContent();
    }
}
