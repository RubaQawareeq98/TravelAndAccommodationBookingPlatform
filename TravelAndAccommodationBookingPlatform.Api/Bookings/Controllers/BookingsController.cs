using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Extensions;
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
    public async Task<IActionResult> GetBooking([FromRoute] Guid bookingId)
    {
        var result = await bookingService.GetBookingWithDetailsById(bookingId);
        return result.Map(bookingResponseMapper.MapBookingWithDetailsToBookingResponse).ToActionResult();
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
        var roomsIds = addBookingRequest.RoomsIds;
        
        var result = await bookingService.AddBooking(booking, roomsIds);
        
        return result.ToActionResult(addedBooking =>
        {
            var bookingResponse = bookingResponseMapper.MapBookingToBookingResponse(addedBooking);
            return CreatedAtAction(nameof(GetBooking),
                new { bookingId = booking.Id }, bookingResponse);
        });
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
        var result = await bookingService.GetBookingById(bookingId);
        if (result.IsFailure)
        {
            return result.ToActionResult();
        }
        var booking = result.Value;
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
        var result = await bookingService.DeleteBooking(bookingId);
        return result.ToActionResult();
    }

    /// <summary>
    /// Generates and returns a PDF invoice for the specified booking.
    /// </summary>
    /// <param name="bookingId">The unique identifier of the booking.</param>
    /// <returns>
    /// A PDF file containing the invoice if the booking exists;
    /// otherwise, a 404 Not Found response.
    /// </returns>
    /// <response code="200">Returns the PDF invoice file.</response>
    /// <response code="404">If the booking does not exist.</response>
    [HttpGet("{bookingId:guid}/invoice")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInvoice([FromRoute] Guid bookingId)
    {
        var result = await bookingService.GenerateInvoiceForBooking(bookingId);

        if (result.IsFailure){
            return result.ToActionResult();
        }
        
        var pdfBytes = result.Value;
        const string fileName = "invoice.pdf";

        return File(pdfBytes, "application/pdf", fileName);
    }

}
