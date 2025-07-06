using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Discounts.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Discounts.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Extensions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Discounts.Controllers;

[Route("api/hotels/{hotelId:guid}/room-categories/{roomCategoryId:guid}/discounts")]
[Authorize]
[ApiController]
public class DiscountsController(
    IDiscountService discountService,
    DiscountRequestMapper discountRequestMapper,
    DiscountResponseMapper discountResponseMapper) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of discounts for a specific hotel and room category with optional filtering, sorting, and pagination.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel.</param>
    /// <param name="roomCategoryId">The ID of the room category.</param>
    /// <param name="sieveModel">Sieve model for filtering, sorting, and pagination.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of discounts.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDiscounts(
        [FromRoute] Guid hotelId,
        [FromRoute] Guid roomCategoryId,
        [FromQuery] SieveModel sieveModel,
        CancellationToken cancellationToken)
    {
        var result = await discountService.GetDiscountsByRoom(hotelId, roomCategoryId, sieveModel, cancellationToken);
        return result.Map(discountResponseMapper.MapDiscountListToDiscountResponseList).ToActionResult();
    }

    /// <summary>
    /// Retrieves a specific discount by ID.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel.</param>
    /// <param name="roomCategoryId">The ID of the room category.</param>
    /// <param name="discountId">The ID of the discount.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Discount details or 404 if not found.</returns>
    [HttpGet("{discountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDiscount(
        [FromRoute] Guid hotelId,
        [FromRoute] Guid roomCategoryId,
        [FromRoute] Guid discountId,
        CancellationToken cancellationToken)
    {
        var result = await discountService.GetDiscountById(hotelId, roomCategoryId, discountId, cancellationToken);
        return result.Map(discountResponseMapper.MapDiscountToDiscountResponse).ToActionResult();
    }

    /// <summary>
    /// Adds a new discount to a specific hotel and room category.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel.</param>
    /// <param name="roomCategoryId">The ID of the room category.</param>
    /// <param name="addDiscountRequest">The discount data to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created discount with a location header.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddDiscount(
        [FromRoute] Guid hotelId,
        [FromRoute] Guid roomCategoryId,
        [FromBody] AddDiscountRequest addDiscountRequest,
        CancellationToken cancellationToken)
    {
        var discount = discountRequestMapper.MapAddDiscountRequestToDiscount(addDiscountRequest);
        await discountService.AddDiscount(hotelId, roomCategoryId, discount, cancellationToken);

        var discountResponse = discountResponseMapper.MapDiscountToDiscountResponse(discount);
        return CreatedAtAction(nameof(GetDiscount),
            new { hotelId, roomCategoryId, discountId = discount.Id },
            discountResponse);
    }

    /// <summary>
    /// Partially updates a discount using a JSON Patch document.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel.</param>
    /// <param name="roomCategoryId">The ID of the room category.</param>
    /// <param name="discountId">The ID of the discount.</param>
    /// <param name="discountPatchDocument">Patch document containing updated fields.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content if updated; 404 if not found; 400 if invalid model.</returns>
    [HttpPatch("{discountId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateDiscount(
        [FromRoute] Guid hotelId,
        [FromRoute] Guid roomCategoryId,
        [FromRoute] Guid discountId,
        [FromBody] JsonPatchDocument<UpdateDiscountRequest> discountPatchDocument,
        CancellationToken cancellationToken)
    {
        var result = await discountService.GetDiscountById(hotelId, roomCategoryId, discountId, cancellationToken);
        if (result.IsFailure)
            return result.ToActionResult();

        var discount = result.Value;
        var updateDiscountRequest = discountRequestMapper.MapDiscountToUpdateDiscountRequest(discount);
        discountPatchDocument.ApplyTo(updateDiscountRequest, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        discountRequestMapper.MapUpdateDiscountRequestToDiscount(updateDiscountRequest, discount);
        await discountService.UpdateDiscount(discount);
        return NoContent();
    }

    /// <summary>
    /// Soft deletes a discount by ID.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel.</param>
    /// <param name="roomCategoryId">The ID of the room category.</param>
    /// <param name="discountId">The ID of the discount to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content if deleted; 404 if not found.</returns>
    [HttpDelete("{discountId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteDiscount(
        [FromRoute] Guid hotelId,
        [FromRoute] Guid roomCategoryId,
        [FromRoute] Guid discountId,
        CancellationToken cancellationToken)
    {
        var result = await discountService.DeleteDiscount(hotelId, roomCategoryId, discountId, cancellationToken);
        return result.ToActionResult();
    }
}
