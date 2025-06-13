using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Cities.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Images.Dtos;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Cities.Controllers;

[Route("api/cities")]
[ApiController]
public class CitiesController(ICityService cityService,
    CityRequestMapper cityRequestMapper,
    IImageService imageService) : ControllerBase
{
    /// <summary>
    /// Return list of cities with pagination, filtering, and sorting
    /// </summary>
    /// <param name="sieveModel"></param>
    /// <returns>list of available cities</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCities([FromQuery] SieveModel sieveModel)
    {
        var cities = await cityService.GetCitiesAsync(sieveModel);
        return Ok(cities);
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
        return Ok(city);
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
        return CreatedAtAction(
            nameof(GetCities),
            new { id = city.Id }, city
            );
    }

    /// <summary>
    /// Uploads and sets a thumbnail image for a city.
    /// </summary>
    /// <param name="cityId">The ID of the city.</param>
    /// <param name="thumbnailImageUploadRequest">The image file to be uploaded.</param>
    /// <returns>The URL of the uploaded image.</returns>
    [HttpPut("{cityId:guid}/thumbnail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddThumbnailToCity([FromRoute] Guid cityId, [FromForm] ThumbnailImageUploadRequest thumbnailImageUploadRequest)
    {
        var city = await cityService.GetCityByIdAsync(cityId);
        
       var url = await imageService.UploadImageAsync(thumbnailImageUploadRequest.File);
        
       city.ThumbnailUrl = url;
       await cityService.UpdateCityAsync(city);
       return Ok(new { imageUrl = url });
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
    /// <param name="patchDoc">The patch document specifying the updates.</param>
    /// <returns>No content on success.</returns>
    [HttpPatch("{cityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCity([FromRoute] Guid cityId, [FromBody] JsonPatchDocument<UpdateCityRequest> patchDoc)
    {
        var city = await cityService.GetCityByIdAsync(cityId);

        var cityRequest = cityRequestMapper.MapCityToUpdateCityRequest(city);
        
        patchDoc.ApplyTo(cityRequest, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        cityRequestMapper.MapUpdateCityRequestToCity(cityRequest, city);
        await cityService.UpdateCityAsync(city);
        
        return NoContent();
    }
}
