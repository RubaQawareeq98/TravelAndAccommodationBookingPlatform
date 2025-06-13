using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

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
    public async Task<ActionResult<Hotel>> GetHotelById([FromQuery] Guid hotelId)
    {
        var hotel = await hotelService.GetHotelByIdAsync(hotelId);
        if (hotel is null)
        {
            return NotFound();
        }
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
        await hotelService.AddHotelAsync(hotel);

        return CreatedAtAction(nameof(GetHotelById), new { hotelId = hotel.Id }, hotel);
    }
}
