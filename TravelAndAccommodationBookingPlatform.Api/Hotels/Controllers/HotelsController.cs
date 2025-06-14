using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Controllers;

[Route("api/hotels")]
[ApiController]
public class HotelsController(IHotelService hotelService, HotelRequestMapper hotelRequestMapper) : ControllerBase
{
    /// <summary>
    /// Get Hotel details by hotel ID
    /// </summary>
    /// <param name="hotelId">hotel id</param>
    /// <response code="200">If the hotel exist.</response>
    /// <response code="404">If the hotel not exist.</response>
    /// <returns>hotel details if hotel id exist</returns>
    [HttpGet("{hotelId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Hotel>> GetHotelById([FromRoute] Guid hotelId)
    {
        var hotel = await hotelService.GetHotelById(hotelId);
        return Ok(hotel);
    }

    /// <summary>
    /// Create a new hotel.
    /// </summary>
    /// <param name="addHotelRequest">The hotel details to create.</param>
    /// <response code="201">Returns the newly created hotel.</response>
    /// <response code="400">If the hotel data is invalid.</response>
    /// <returns>The created hotel with location header.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Hotel>> CreateHotel(AddHotelRequest addHotelRequest)
    {
        var hotel = hotelRequestMapper.MapHotelRequestToHotel(addHotelRequest);
        await hotelService.AddHotel(hotel);

        return CreatedAtAction(nameof(GetHotelById), new { hotelId = hotel.Id }, hotel);
    }
    
    /// <summary>
    /// Applies a partial update to a hotel using a JSON Patch document.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel to update.</param>
    /// <param name="hotelPatchDoc">The patch document specifying the updates.</param>
    /// <returns>No content on success.</returns>
    [HttpPatch("{hotelId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHotel([FromRoute] Guid hotelId, [FromBody] JsonPatchDocument<UpdateHotelRequest> hotelPatchDoc)
    {
        var hotel = await hotelService.GetHotelById(hotelId);

        var hotelRequest = hotelRequestMapper.MapHotelToUpdateHotelRequest(hotel);
        
        hotelPatchDoc.ApplyTo(hotelRequest, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        hotelRequestMapper.MapUpdateHotelRequestToHotel(hotelRequest, hotel);
        await hotelService.UpdateHotel(hotel);
        
        return NoContent();
    }
}
