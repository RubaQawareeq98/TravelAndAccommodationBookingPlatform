using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Cities.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Cities.Controllers;

[Route("api/cities")]
[ApiController]
public class CitiesController(ICityService cityService, CityRequestMapper cityRequestMapper) : ControllerBase
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
}
