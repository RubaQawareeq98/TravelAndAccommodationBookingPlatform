using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Owners.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Owners.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Owners.Controllers;

[Route("api/owners")]
[ApiController]
public class OwnersController(IOwnerService ownerService, OwnerRequestMapper ownerRequestMapper) : ControllerBase
{
    /// <summary>
    /// Return list of owners with pagination, filtering, sorting
    /// </summary>
    /// <param name="sieveModel"></param>
    /// <returns>list of available owners</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Owner>>> GetOwners([FromQuery] SieveModel sieveModel)
    {
        var owners = await ownerService.GetOwnersAsync(sieveModel);
        return Ok(owners);
    }

    /// <summary>
    /// Return owner by owner id if owner id exist
    /// </summary>
    /// <param name="ownerId"></param>
    ///  /// <response code="200">If the owner exist.</response>
    /// <response code="404">If the owner not exist.</response>
    /// <returns>owner if exist or not found</returns>
    [HttpGet("{ownerId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Owner>> GetOwner([FromRoute] Guid ownerId)
    {
        var owner = await ownerService.GetOwnerByIdAsync(ownerId);
        return Ok(owner);
    }

    /// <summary>
    /// Add new owner with valid data
    /// </summary>
    /// <param name="addOwnerRequest"></param>
    /// <response code="201">If the owner created.</response>
    /// <response code="400">If the owner data not valid.</response>
    /// <returns>created owner</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddOwner([FromBody] AddOwnerRequest addOwnerRequest)
    {
        var owner = ownerRequestMapper.MapAddOwnerRequestToOwner(addOwnerRequest);
        await ownerService.AddOwnerAsync(owner);
        
        return CreatedAtAction(nameof(GetOwner),
            new { ownerId = owner.Id }, owner);
    }
    
    /// <summary>
    /// Partially update the owner information
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="ownerPatchDocument"></param>
    /// <response code="204">If owner updated successfully.</response>
    /// <response code="404">If the owner not exist.</response>
    /// <returns>No content if updated successfully or not found.</returns>
    [HttpPatch("{ownerId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOwner([FromRoute] Guid ownerId, JsonPatchDocument<UpdateOwnerRequest> ownerPatchDocument)
    {
        var owner = await ownerService.GetOwnerByIdAsync(ownerId);

        var updateOwnerRequest = ownerRequestMapper.MapOwnerToUpdateOwnerRequest(owner);
        ownerPatchDocument.ApplyTo(updateOwnerRequest);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        ownerRequestMapper.MapUpdateOwnerRequestToOwner(updateOwnerRequest, owner);
        
        await ownerService.UpdateOwnerAsync(owner);
        return NoContent();
    }

    /// <summary>
    /// Soft delete owner by owner id
    /// </summary>
    /// <param name="ownerId"></param>
    /// <response code="204">If the owner deleted successfully.</response>
    /// <response code="404">If the hotel not exist.</response>
    /// <returns>No content if owner deleted successfully or not found.</returns>
    [HttpDelete("{ownerId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOwner([FromRoute] Guid ownerId)
    {
        await ownerService.DeleteOwnerAsync(ownerId);
        return NoContent();
    }
}
