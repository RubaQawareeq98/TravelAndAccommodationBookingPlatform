using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Amenities.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Amenities.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Api.Amenities.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Extensions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Amenities.Controllers;

[Route("api/amenities")]
[Authorize]
[ApiController]
public class AmenitiesController(IAmenityService amenityService,
    AmenityRequestMapper amenityRequestMapper,
    AmenityResponseMapper amenityResponseMapper) : ControllerBase
{
    /// <summary>
    /// Return list of amenities with pagination, filtering, sorting
    /// </summary>
    /// <param name="sieveModel"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>list of available amenities</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AmenityResponse>>> GetAmenities([FromQuery] SieveModel sieveModel , CancellationToken cancellationToken)
    {
        var amenities = await amenityService.GetAmenities(sieveModel, cancellationToken);
        var amenitiesResponse = amenityResponseMapper.MapAmenityListToAmenityResponseList(amenities);
        return Ok(amenitiesResponse);
    }

    /// <summary>
    /// Return amenity by amenity id if amenity id exist
    /// </summary>
    /// <param name="amenityId"></param>
    /// <param name="cancellationToken"></param>
    /// /// <response code="200">If the amenity exist.</response>
    /// <response code="404">If the amenity not exist.</response>
    /// <returns>amenity if exist or not found</returns>
    [HttpGet("{amenityId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmenity([FromRoute] Guid amenityId , CancellationToken cancellationToken)
    {
        var result = await amenityService.GetAmenityById(amenityId, cancellationToken);
        return result.Map(amenityResponseMapper.MapAmenityToAmenityResponse).ToActionResult();
    }

    /// <summary>
    /// Add new amenity with valid data
    /// </summary>
    /// <param name="addAmenityRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="201">If the amenity created.</response>
    /// <response code="400">If the amenity data not valid.</response>
    /// <returns>created amenity</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAmenity([FromBody] AddAmenityRequest addAmenityRequest , CancellationToken cancellationToken)
    {
        var amenity = amenityRequestMapper.MapAddAmenityRequestToAmenity(addAmenityRequest);
        var result = await amenityService.AddAmenity(amenity, cancellationToken);
        if (result.IsFailure)
        {
            return result.ToActionResult();
        }
        
        var amenityResponse = amenityResponseMapper.MapAmenityToAmenityResponse(amenity);
        
        return CreatedAtAction(nameof(GetAmenity),
            new { amenityId = amenity.Id },
            amenityResponse);
    }

    /// <summary>
    /// Partially update the amenity information
    /// </summary>
    /// <param name="amenityId"></param>
    /// <param name="amenityPatchDocument"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">If amenity updated successfully.</response>
    /// <response code="404">If the amenity not exist.</response>
    /// <returns>No content if updated successfully or not found.</returns>
    [HttpPatch("{amenityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAmenity([FromRoute] Guid amenityId, JsonPatchDocument<UpdateAmenityRequest> amenityPatchDocument , CancellationToken cancellationToken)
    {
        var result = await amenityService.GetAmenityById(amenityId, cancellationToken);
        if (result.IsFailure)
        {
            return result.ToActionResult();
        }
        
        var amenity = result.Value;
        var updateAmenityRequest = amenityRequestMapper.MapAmenityToUpdateAmenityRequest(amenity);
        amenityPatchDocument.ApplyTo(updateAmenityRequest);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        amenityRequestMapper.MapUpdateAmenityRequestToAmenity(updateAmenityRequest, amenity);
        
        await amenityService.UpdateAmenity(amenity, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Delete amenity by amenity id
    /// </summary>
    /// <param name="amenityId"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">If the amenity deleted successfully.</response>
    /// <response code="404">If the hotel not exist.</response>
    /// <returns>No content if amenity deleted successfully or not found.</returns>
    [HttpDelete("{amenityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAmenity([FromRoute] Guid amenityId , CancellationToken cancellationToken)
    {
        var result = await amenityService.DeleteAmenity(amenityId, cancellationToken);
        return result.ToActionResult();
    }
}
