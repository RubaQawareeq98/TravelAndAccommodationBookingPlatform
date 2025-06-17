using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.Api.RoomInfos.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.RoomInfos.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Api.RoomInfos.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.RoomInfos.Controllers;

[Route("api/roomInfos")]
[ApiController]
public class RoomInfosController(IRoomInfoService roomInfoService,
    RoomInfoRequestMapper roomInfoRequestMapper,
    RoomInfoResponseMapper roomInfoResponseMapper) : ControllerBase
{
    /// <summary>
    /// Return list of roomInfos with pagination, filtering, sorting
    /// </summary>
    /// <returns>list of available roomInfos</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RoomInfoResponse>>> GetRoomInfos()
    {
        var roomInfos = await roomInfoService.GetRoomInfos();
        var roomInfosResponse = roomInfoResponseMapper.MapRoomInfoListToRoomInfoResponseList(roomInfos);
        return Ok(roomInfosResponse);
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
    public async Task<ActionResult<RoomInfoResponse>> GetRoomInfo([FromRoute] Guid roomInfoId)
    {
        var roomInfo = await roomInfoService.GetRoomInfoById(roomInfoId);
        var roomInfoResponse = roomInfoResponseMapper.MapRoomInfoToRoomInfoResponse(roomInfo);
        return Ok(roomInfoResponse);
    }

    /// <summary>
    /// Add new roomInfo with valid data
    /// </summary>
    /// <param name="addRoomInfoRequest"></param>
    /// <response code="201">If the roomInfo created.</response>
    /// <response code="400">If the roomInfo data not valid.</response>
    /// <response code="404">If one of the amenities ID not found.</response>
    /// <returns>created roomInfo</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddRoomInfo([FromBody] AddRoomInfoRequest addRoomInfoRequest)
    {
        var roomInfo = roomInfoRequestMapper.MapAddRoomInfoRequestToRoomInfo(addRoomInfoRequest);
        var amenitiesIds = addRoomInfoRequest.AmenitiesIds ?? [];
        
        await roomInfoService.AddRoomInfo(roomInfo, amenitiesIds);
        
        var roomInfoResponse = roomInfoResponseMapper.MapRoomInfoToRoomInfoResponse(roomInfo);
        return CreatedAtAction(nameof(GetRoomInfo),
            new { roomInfoId = roomInfo.Id }, roomInfoResponse);
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
        var roomInfo = await roomInfoService.GetRoomInfoById(roomInfoId);

        var updateRoomInfoRequest = roomInfoRequestMapper.MapRoomInfoToUpdateRoomInfoRequest(roomInfo);
        roomInfoPatchDocument.ApplyTo(updateRoomInfoRequest);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        roomInfoRequestMapper.MapUpdateRoomInfoRequestToRoomInfo(updateRoomInfoRequest, roomInfo);
        
        await roomInfoService.UpdateRoomInfo(roomInfo);
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
        await roomInfoService.DeleteRoomInfo(roomInfoId);
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
        var sieveModel = roomInfoRequestMapper.MapSearchCritereaToSieveModel(searchRequest);
        var rooms = await roomInfoService.GetFilteredRooms(sieveModel, searchRequest.AmenitiesIds);

        var roomInfos = roomInfoResponseMapper.MapRoomInfoListToRoomInfoResponseList(rooms);
        return Ok(roomInfos);
    }
}
