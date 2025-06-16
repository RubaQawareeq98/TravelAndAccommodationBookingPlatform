using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Api.Cities.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Images.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Cities.Controllers;

[Route("api/cities")]
[ApiController]
public class CitiesController(ICityService cityService,
    CityRequestMapper cityRequestMapper,
    CityResponseMapper cityResponseMapper) : ControllerBase
{
    /// <summary>
    /// Return list of cities with pagination, filtering, and sorting
    /// </summary>
    /// <param name="sieveModel"></param>
    /// <returns>list of available cities</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CityResponse>> GetCities([FromQuery] SieveModel sieveModel)
    {
        var cities = await cityService.GetCitiesAsync(sieveModel);
        var citiesList = cityResponseMapper.MapCityListToCityResponseList(cities);
        return Ok(citiesList);
    }

    /// <summary>
    /// Retrieves a city by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the city.</param>
    /// <returns>The city object.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCityById([FromRoute] Guid id)
    {
        var city = await cityService.GetCityByIdAsync(id);
        var cityResponse = cityResponseMapper.MapCityToCityResponse(city);
        return Ok(cityResponse);
    }

    /// <summary>
    /// Adds a new city.
    /// </summary>
    /// <param name="request">The details of the city to be added.</param>
    /// <returns>The created city with its ID.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddCities([FromBody] AddCityRequest request)
    {
        var city = cityRequestMapper.MapCityRequestToCity(request);
        await cityService.AddCityAsync(city);
        
        var cityResponse = cityResponseMapper.MapCityToCityResponse(city);
        return CreatedAtAction(
            nameof(GetCities),
            new { id = city.Id }, cityResponse
            );
    }

    /// <summary>
    /// Deletes a city by its ID.
    /// </summary>
    /// <param name="cityId">The ID of the city to delete.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{cityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCity([FromRoute] Guid cityId)
    {
        await cityService.DeleteCityAsync(cityId);
        return NoContent();
    }

    /// <summary>
    /// Applies a partial update to a city using a JSON Patch document.
    /// </summary>
    /// <param name="cityId">The ID of the city to update.</param>
    /// <param name="cityPatchDoc">The patch document specifying the updates.</param>
    /// <returns>No content on success.</returns>
    [HttpPatch("{cityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCity([FromRoute] Guid cityId, [FromBody] JsonPatchDocument<UpdateCityRequest> cityPatchDoc)
    {
        var city = await cityService.GetCityByIdAsync(cityId);

        var cityRequest = cityRequestMapper.MapCityToUpdateCityRequest(city);
        
        cityPatchDoc.ApplyTo(cityRequest, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        cityRequestMapper.MapUpdateCityRequestToCity(cityRequest, city);
        await cityService.UpdateCityAsync(city);
        
        return NoContent();
    }
    
    /// <summary>
    /// Uploads and sets a thumbnail image for a city.
    /// </summary>
    /// <param name="cityId">The ID of the city.</param>
    /// <param name="imageUploadRequest">The image file to be uploaded.</param>
    /// <returns>The URL of the uploaded image.</returns>
    [HttpPut("{cityId:guid}/thumbnail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddThumbnailToCity([FromRoute] Guid cityId, [FromForm] ImageUploadRequest imageUploadRequest)
    {
        var url = await cityService.UpdateCityThumbnail(cityId, imageUploadRequest.File);
        return Ok(new { imageUrl = url });
    }

    [HttpGet("trending")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrendingCities([FromQuery] int listCount = 5, CancellationToken cancellationToken = default)
    {
        var cities = await cityService.GetTrendingCities(listCount, cancellationToken);
        
        var citiesList = cityResponseMapper.MapCityListToCityResponseList(cities);
        return Ok(citiesList);
    }
}
