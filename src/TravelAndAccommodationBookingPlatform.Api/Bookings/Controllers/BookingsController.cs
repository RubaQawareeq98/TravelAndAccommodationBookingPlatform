using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Extensions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Bookings.Controllers;

[Route("api/users/{userId:guid}/bookings")]
[ApiController]
public class BookingsController(IBookingService bookingService,
    BookingRequestMapper bookingRequestMapper,
    BookingResponseMapper bookingResponseMapper) : ControllerBase
{
    /// <summary>
    /// Return list of user bookings with pagination, filtering, sorting
    /// </summary>
    /// <param name="sieveModel"></param>
    /// <param name="userId">The ID of user to fetch his bookings</param>
    /// <param name="cancellationToken"></param>
    /// <returns>list of user bookings or not found</returns>
    /// <response code="404">If the user not exist.</response>
    /// <response code="200">If the user exist.</response>
    /// <response code="401">If the user not authorized.</response>
    /// <response code="403">If the user not allowed to fetch bookings.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetBookings(
        [FromRoute] Guid userId,
        [FromQuery] SieveModel sieveModel,
        CancellationToken cancellationToken)
    {
        var result = await bookingService.GetBookings(sieveModel, userId, cancellationToken);
        if (result.IsFailure)
        {
            return result.ToActionResult();
        }
        
        var bookings = result.Value;
        var bookingList = bookingResponseMapper.MapBookingListToBookingResponseList(bookings);
        return Ok(bookingList);
    }

    /// <summary>
    /// Return booking by booking id if booking id exist
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="bookingId"></param>
    /// <param name="cancellationToken"></param>
    /// /// <response code="200">If the booking exist.</response>
    /// <response code="404">If the user not exist.</response>
    /// <response code="404">If the booking not exist.</response>
    /// <returns>booking if exist or not found</returns>
    [HttpGet("{bookingId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetBookingById([FromRoute] Guid userId,
        [FromRoute] Guid bookingId,
        CancellationToken cancellationToken)
    {
        var result = await bookingService.GetBookingWithDetailsById(userId, bookingId, cancellationToken);
        return result.Map(bookingResponseMapper.MapBookingWithDetailsToBookingResponse).ToActionResult();
    }

    /// <summary>
    /// Add new booking with valid data
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="addBookingRequest"></param>
    /// <param name="cancellationToken"></param>.
    /// <returns>created booking</returns>
    /// <response code="201">If the booking created.</response>
    /// <response code="400">If the booking data not valid.</response>
    /// <response code="404">If the hotelId not found.</response>
    /// <response code="404">If the userId not found.</response>
    /// <response code="401">If the user not authorized.</response>
    /// <response code="403">If the user sent request not same authorized user.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddBooking(
        [FromRoute] Guid userId,
        [FromBody] AddBookingRequest addBookingRequest,
        CancellationToken cancellationToken)
    {
        var booking = bookingRequestMapper.MapAddBookingRequestToBooking(addBookingRequest);
        var roomsIds = addBookingRequest.RoomsIds;
        
        var result = await bookingService.AddBooking(userId, booking, roomsIds, cancellationToken);
        
        return result.ToActionResult(addedBooking =>
        {
            var bookingResponse = bookingResponseMapper.MapBookingToBookingResponse(addedBooking);
            return CreatedAtAction(nameof(GetBookingById),
                new { bookingId = booking.Id }, bookingResponse);
        });
    }

    /// <summary>
    /// Partially update the booking information
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="bookingId"></param>
    /// <param name="bookingPatchDocument"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">If booking updated successfully.</response>
    /// <response code="404">If the user not exist.</response>
    /// <response code="404">If the booking not exist.</response>
    /// <response code="401">If the user not authorized.</response>
    /// <response code="403">If the user sent request not same authorized user.</response>
    /// <returns>No content if updated successfully or not found.</returns>
    [HttpPatch("{bookingId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateBooking(
        [FromRoute] Guid userId,
        [FromRoute] Guid bookingId,
        JsonPatchDocument<UpdateBookingRequest> bookingPatchDocument,
        CancellationToken cancellationToken)
    {
        var result = await bookingService.GetBookingById(userId, bookingId, cancellationToken);
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
    /// <param name="userId"></param>
    /// <param name="bookingId"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">If the booking deleted successfully.</response>
    /// <response code="404">If the user not exist.</response>
    /// <response code="404">If the booking not exist.</response>
    /// <response code="401">If the user not authorized.</response>
    /// <response code="403">If the user sent request not same authorized user.</response>
    /// <returns>No content if booking deleted successfully or not found.</returns>
    [HttpDelete("{bookingId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteBooking([FromRoute] Guid userId,
        [FromRoute] Guid bookingId,
        CancellationToken cancellationToken)
    {
        var result = await bookingService.DeleteBooking(userId, bookingId, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Generates and returns a PDF invoice for the specified booking.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="bookingId">The unique identifier of the booking.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// A PDF file containing the invoice if the booking exists;
    /// otherwise, a 404 Not Found response.
    /// </returns>
    /// <response code="200">Returns the PDF invoice file.</response>
    /// <response code="404">If the user does not exist.</response>
    /// <response code="404">If the booking does not exist.</response>
    /// <response code="401">If the user not authorized.</response>
    [HttpGet("{bookingId:guid}/invoice")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInvoice([FromRoute] Guid userId,
        [FromRoute] Guid bookingId,
        CancellationToken cancellationToken)
    {
        var result = await bookingService.GenerateInvoiceForBooking(userId, bookingId, cancellationToken);

        if (result.IsFailure){
            return result.ToActionResult();
        }
        
        var pdfBytes = result.Value;
        const string fileName = "invoice.pdf";

        return File(pdfBytes, "application/pdf", fileName);
    }
}
