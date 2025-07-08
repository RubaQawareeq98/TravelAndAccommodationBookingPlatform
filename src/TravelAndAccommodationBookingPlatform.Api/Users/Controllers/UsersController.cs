using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.Api.Extensions;
using TravelAndAccommodationBookingPlatform.Api.Users.Dtos;
using TravelAndAccommodationBookingPlatform.Api.Users.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Users.Mappers.Extensions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Users.Controllers;

[Route("api/users")]
[Authorize]
[ApiController]
public class UsersController(IBookingService bookingService, RecentBookingsToHotelsMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get recently visited hotels by userId
    /// </summary>
    /// <param name="userId">The id of user wants to retrieve recently visited hotels</param>
    /// <param name="recentlyVisitedRequest">The number of recently visited hotels.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of hotels with details and city data with payment data</returns>
    [HttpGet("{userId:guid}/recently-visited")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRecentlyVisitedHotelsByUserId([FromRoute] Guid userId,
        [FromQuery] GetRecentlyVisitedRequest recentlyVisitedRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await bookingService.GetRecentlyVisitedHotels(userId, recentlyVisitedRequest.ListCount, cancellationToken);
        if (result.IsFailure)
        {
            return result.ToActionResult();
        }

        var hotels = result.Value;
        var response = hotels.
            Select(mapper.MapWithCity)
            .ToList();
        
        return Ok(response);
    }
}
