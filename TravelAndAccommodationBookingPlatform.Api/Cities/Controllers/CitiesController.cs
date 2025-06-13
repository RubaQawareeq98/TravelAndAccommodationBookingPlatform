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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCities([FromQuery] Guid id)
    {
        var city = await cityService.GetCityByIdAsync(id);
        return Ok(city);
    }

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

    [HttpPost("{cityId:guid}/thumbnail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddThumbnailToCity([FromRoute] Guid cityId, [FromForm] ThumbnailImageUploadRequest thumbnailImageUploadRequest)
    {
        var city = await cityService.GetCityByIdAsync(cityId);
        
       var url = await imageService.UploadImageAsync(thumbnailImageUploadRequest.File);
        
       city.ThumbnailUrl = url;
       await cityService.UpdateCityAsync(city);
       return Ok(new { imageUrl = url });
    }
}
