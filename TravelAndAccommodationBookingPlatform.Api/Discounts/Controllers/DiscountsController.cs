using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Discounts.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Discounts.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Discounts.Controllers;

public class DiscountsController(IDiscountService discountService, DiscountRequestMapper discountRequestMapper) : ControllerBase
{
    /// <summary>
    /// Return list of discounts with pagination, filtering, sorting
    /// </summary>
    /// <param name="sieveModel"></param>
    /// <returns>list of available discounts</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Discount>>> GetDiscounts([FromQuery] SieveModel sieveModel)
    {
        var discounts = await discountService.GetDiscounts(sieveModel);
        return Ok(discounts);
    }

    /// <summary>
    /// Return discount by discount id if discount id exist
    /// </summary>
    /// <param name="discountId"></param>
    ///  /// <response code="200">If the discount exist.</response>
    /// <response code="404">If the discount not exist.</response>
    /// <returns>discount if exist or not found</returns>
    [HttpGet("{discountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Discount>> GetDiscount([FromRoute] Guid discountId)
    {
        var discount = await discountService.GetDiscountById(discountId);
        return Ok(discount);
    }

    /// <summary>
    /// Add new discount with valid data
    /// </summary>
    /// <param name="addDiscountRequest"></param>
    /// <response code="201">If the discount created.</response>
    /// <response code="400">If the discount data not valid.</response>
    /// <returns>created discount</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddDiscount([FromBody] AddDiscountRequest addDiscountRequest)
    {
        var discount = discountRequestMapper.MapAddDiscountRequestToDiscount(addDiscountRequest);
        await discountService.AddDiscount(discount);
        
        return CreatedAtAction(nameof(GetDiscount),
            new { discountId = discount.Id }, discount);
    }
    
    /// <summary>
    /// Partially update the discount information
    /// </summary>
    /// <param name="discountId"></param>
    /// <param name="discountPatchDocument"></param>
    /// <response code="204">If discount updated successfully.</response>
    /// <response code="404">If the discount not exist.</response>
    /// <returns>No content if updated successfully or not found.</returns>
    [HttpPatch("{discountId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDiscount([FromRoute] Guid discountId, JsonPatchDocument<UpdateDiscountRequest> discountPatchDocument)
    {
        var discount = await discountService.GetDiscountById(discountId);

        var updateDiscountRequest = discountRequestMapper.MapDiscountToUpdateDiscountRequest(discount);
        discountPatchDocument.ApplyTo(updateDiscountRequest);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        discountRequestMapper.MapUpdateDiscountRequestToDiscount(updateDiscountRequest, discount);
        
        await discountService.UpdateDiscount(discount);
        return NoContent();
    }

    /// <summary>
    /// Soft delete discount by discount id
    /// </summary>
    /// <param name="discountId"></param>
    /// <response code="204">If the discount deleted successfully.</response>
    /// <response code="404">If the hotel not exist.</response>
    /// <returns>No content if discount deleted successfully or not found.</returns>
    [HttpDelete("{discountId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDiscount([FromRoute] Guid discountId)
    {
        await discountService.DeleteDiscount(discountId);
        return NoContent();
    }
}
