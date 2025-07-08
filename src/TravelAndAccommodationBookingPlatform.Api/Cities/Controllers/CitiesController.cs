using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Api.Cities.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Extensions;
using TravelAndAccommodationBookingPlatform.Api.Images.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Cities.Controllers;

[Route("api/cities")]
//[Authorize(Roles = "Admin")]
[ApiController]
public class CitiesController(
    ICityService cityService,
    CityRequestMapper cityRequestMapper,
    CityResponseMapper cityResponseMapper) : ControllerBase
{
    /// <summary>
    /// Gets a list of cities with support for pagination, filtering, and sorting.
    /// </summary>
    /// <param name="sieveModel">Query parameters for filtering, sorting, and pagination.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of cities matching the given criteria.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CityResponse>> GetCities([FromQuery] SieveModel sieveModel, CancellationToken cancellationToken)
    {
        var cities = await cityService.GetCities(sieveModel, cancellationToken);
        var citiesList = cityResponseMapper.MapCityListToCityResponseList(cities);
        return Ok(citiesList);
    }

    /// <summary>
    /// Retrieves a specific city by its ID.
    /// </summary>
    /// <param name="id">The ID of the city.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The city details if found, otherwise a 404.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCityById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await cityService.GetCityById(id, cancellationToken);
        return result.Map(cityResponseMapper.MapCityToCityResponse).ToActionResult();
    }

    /// <summary>
    /// Creates a new city.
    /// </summary>
    /// <param name="request">The new city data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created city with its generated ID.</returns>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddCity([FromBody] AddCityRequest request, CancellationToken cancellationToken)
    {
        var city = cityRequestMapper.MapCityRequestToCity(request);
    
        var result = await cityService.AddCity(city, cancellationToken);

        return result.ToActionResult(addedCity =>
        {
            var cityResponse = cityResponseMapper.MapCityToCityResponse(addedCity);
            return CreatedAtAction(nameof(GetCityById), new { id = addedCity.Id }, cityResponse);
        });
    }

    /// <summary>
    /// Deletes a city by ID.
    /// </summary>
    /// <param name="cityId">The ID of the city to delete.</param>
    /// <returns>No content if successful; 404 if city not found.</returns>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{cityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteCity([FromRoute] Guid cityId)
    {
        var result = await cityService.DeleteCity(cityId);
        return result.ToActionResult();
    }

    /// <summary>
    ///Partially updates an existing city partially using a JSON Patch document.
    /// </summary>
    /// <param name="cityId">ID of the city to update.</param>
    /// <param name="cityPatchDoc">The patch document with the changes.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content on success, or appropriate error response.</returns>
    [HttpPatch("{cityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateCity(
        [FromRoute] Guid cityId,
        [FromBody] JsonPatchDocument<UpdateCityRequest> cityPatchDoc,
        CancellationToken cancellationToken)
    {
        var cityResult = await cityService.GetCityById(cityId, cancellationToken);
        if (cityResult.IsFailure)
        {
            return cityResult.ToActionResult();
        }
        
        var city = cityResult.Value;
        var cityRequest = cityRequestMapper.MapCityToUpdateCityRequest(city);

        cityPatchDoc.ApplyTo(cityRequest, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var isValid = TryValidateModel(cityRequest);
        if (!isValid)
        {
            return ValidationProblem(ModelState);
        }

        cityRequestMapper.MapUpdateCityRequestToCity(cityRequest, city);
        await cityService.UpdateCity(city, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Uploads a new thumbnail image for a specific city.
    /// </summary>
    /// <param name="cityId">The ID of the city.</param>
    /// <param name="imageUploadRequest">The uploaded image file.</param>
    /// <returns>The URL of the newly uploaded image.</returns>
    [HttpPut("{cityId:guid}/thumbnail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddThumbnailToCity([FromRoute] Guid cityId, [FromForm] ImageUploadRequest imageUploadRequest)
    {
        var result = await cityService.UpdateCityThumbnail(cityId, imageUploadRequest.File);
        return result.IsFailure ? result.ToActionResult() : Ok(new { imageUrl = result.Value.ThumbnailUrl });
    }

    /// <summary>
    /// Gets a list of trending cities based on popularity or predefined criteria.
    /// </summary>
    /// <param name="trendingCitiesRequest">Request containing the number of cities to return.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of trending cities.</returns>
    [HttpGet("trending")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTrendingCities([FromQuery] GetTrendingCitiesRequest trendingCitiesRequest, CancellationToken cancellationToken = default)
    {
        var cities = await cityService.GetTrendingCities(trendingCitiesRequest.ListCount, cancellationToken);
        var citiesList = cityResponseMapper.MapCityListToCityResponseList(cities);
        return Ok(citiesList);
    }
}
