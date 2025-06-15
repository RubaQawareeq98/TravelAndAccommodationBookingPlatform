using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Bookings.Controllers;

[Route("api/bookings")]
[ApiController]
public class BookingsController(IBookingService bookingService,
    BookingRequestMapper bookingRequestMapper,
    BookingResponseMapper bookingResponseMapper) : ControllerBase
{
    /// <summary>
    /// Return list of bookings with pagination, filtering, sorting
    /// </summary>
    /// <param name="sieveModel"></param>
    /// <returns>list of available bookings</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Booking>>> GetBookings([FromQuery] SieveModel sieveModel)
    {
        var bookings = await bookingService.GetBookings(sieveModel);
        var bookingList = bookingResponseMapper.MapBookingListToBookingResponseList(bookings);
        return Ok(bookingList);
    }

    /// <summary>
    /// Return booking by booking id if booking id exist
    /// </summary>
    /// <param name="bookingId"></param>
    ///  /// <response code="200">If the booking exist.</response>
    /// <response code="404">If the booking not exist.</response>
    /// <returns>booking if exist or not found</returns>
    [HttpGet("{bookingId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Booking>> GetBooking([FromRoute] Guid bookingId)
    {
        var booking = await bookingService.GetBookingById(bookingId);
        var bookingResponse = bookingResponseMapper.MapBookingToBookingResponse(booking);
        return Ok(bookingResponse);
    }

    /// <summary>
    /// Add new booking with valid data
    /// </summary>
    /// <param name="addBookingRequest"></param>
    /// <response code="201">If the booking created.</response>
    /// <response code="400">If the booking data not valid.</response>
    /// <returns>created booking</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddBooking([FromBody] AddBookingRequest addBookingRequest)
    {
        var booking = bookingRequestMapper.MapAddBookingRequestToBooking(addBookingRequest);
        await bookingService.AddBooking(booking);
        
        var bookingResponse = bookingResponseMapper.MapBookingToBookingResponse(booking);
        return CreatedAtAction(nameof(GetBooking),
            new { bookingId = booking.Id }, bookingResponse);
    }
    
    /// <summary>
    /// Partially update the booking information
    /// </summary>
    /// <param name="bookingId"></param>
    /// <param name="bookingPatchDocument"></param>
    /// <response code="204">If booking updated successfully.</response>
    /// <response code="404">If the booking not exist.</response>
    /// <returns>No content if updated successfully or not found.</returns>
    [HttpPatch("{bookingId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBooking([FromRoute] Guid bookingId, JsonPatchDocument<UpdateBookingRequest> bookingPatchDocument)
    {
        var booking = await bookingService.GetBookingById(bookingId);

        var updateBookingRequest = bookingRequestMapper.MapBookingToUpdateBookingRequest(booking);
        bookingPatchDocument.ApplyTo(updateBookingRequest);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        bookingRequestMapper.MapUpdateBookingRequestToBooking(updateBookingRequest, booking);
        
        await bookingService.UpdateBooking(booking);
        return NoContent();
    }

    /// <summary>
    /// Soft delete booking by booking id
    /// </summary>
    /// <param name="bookingId"></param>
    /// <response code="204">If the booking deleted successfully.</response>
    /// <response code="404">If the hotel not exist.</response>
    /// <returns>No content if booking deleted successfully or not found.</returns>
    [HttpDelete("{bookingId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBooking([FromRoute] Guid bookingId)
    {
        await bookingService.DeleteBooking(bookingId);
        return NoContent();
    }
}
