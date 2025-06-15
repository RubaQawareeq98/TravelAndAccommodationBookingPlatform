using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers.Extensions;
using TravelAndAccommodationBookingPlatform.Api.Images.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Images.Dtos.Response;
using TravelAndAccommodationBookingPlatform.Api.Images.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Controllers;

[Route("api/hotels")]
[ApiController]
public class HotelsController(IHotelService hotelService,
    HotelRequestMapper hotelRequestMapper,
    HotelResponseMapper hotelResponseMapper,
    GalleryImageMapper galleryImageMapper) : ControllerBase
{
    /// <summary>
    /// Return list of hotels with pagination, filtering, and sorting
    /// </summary>
    /// <param name="sieveModel"></param>
    /// <returns>list of available hotels</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Hotel>> GetHotels([FromQuery] SieveModel sieveModel)
    {
        var hotels = await hotelService.GetHotels(sieveModel);
        var hotelsList = hotelResponseMapper.MapHotelListToHotelResponseList(hotels);
        return Ok(hotelsList);
    }
    
    /// <summary>
    /// Get Hotel details by hotel ID
    /// </summary>
    /// <param name="hotelId">hotel id</param>
    /// <response code="200">If the hotel exist.</response>
    /// <response code="404">If the hotel not exist.</response>
    /// <returns>hotel details if hotel id exist</returns>
    [HttpGet("{hotelId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HotelResponse>> GetHotelById([FromRoute] Guid hotelId)
    {
        var hotel = await hotelService.GetHotelById(hotelId);
        var hotelResponse = hotelResponseMapper.MapHotelToHotelResponse(hotel);
        return Ok(hotelResponse);
    }

    /// <summary>
    /// Create a new hotel.
    /// </summary>
    /// <param name="addHotelRequest">The hotel details to create.</param>
    /// <response code="201">Returns the newly created hotel.</response>
    /// <response code="400">If the hotel data is invalid.</response>
    /// <returns>The created hotel with location header.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Hotel>> CreateHotel(AddHotelRequest addHotelRequest)
    {
        var hotel = hotelRequestMapper.MapHotelRequestToHotel(addHotelRequest);
        await hotelService.AddHotel(hotel);

        var hotelResponse = hotelResponseMapper.MapHotelToHotelResponse(hotel);
        return CreatedAtAction(nameof(GetHotelById),
            new { hotelId = hotel.Id }, hotelResponse);
    }
    
    /// <summary>
    /// Applies a partial update to a hotel using a JSON Patch document.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel to update.</param>
    /// <param name="hotelPatchDoc">The patch document specifying the updates.</param>
    /// <returns>No content on success.</returns>
    [HttpPatch("{hotelId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHotel([FromRoute] Guid hotelId, [FromBody] JsonPatchDocument<UpdateHotelRequest> hotelPatchDoc)
    {
        var hotel = await hotelService.GetHotelById(hotelId);

        var hotelRequest = hotelRequestMapper.MapHotelToUpdateHotelRequest(hotel);
        
        hotelPatchDoc.ApplyTo(hotelRequest, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        hotelRequestMapper.MapUpdateHotelRequestToHotel(hotelRequest, hotel);
        await hotelService.UpdateHotel(hotel);
        
        return NoContent();
    }
    
    /// <summary>
    /// Uploads and sets a thumbnail image for a hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel.</param>
    /// <param name="imageUploadRequest">The image file to be uploaded.</param>
    /// <returns>The URL of the uploaded image.</returns>
    [HttpPut("{hotelId:guid}/thumbnail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddThumbnailToHotel([FromRoute] Guid hotelId, [FromForm] ImageUploadRequest imageUploadRequest)
    {
        var url = await hotelService.UpdateHotelThumbnail(hotelId, imageUploadRequest.File);
        return Ok(new { imageUrl = url });
    }
    
    /// <summary>
    /// Uploads a gallery image for a hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel.</param>
    /// <param name="imageUploadRequest">The image file to be uploaded.</param>
    /// <returns>The URL of the uploaded image.</returns>
    [HttpPost("{hotelId:guid}/gallery")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddImageGalleryToHotel([FromRoute] Guid hotelId, [FromForm] ImageUploadRequest imageUploadRequest)
    {
        var url = await hotelService.AddHotelGallery(hotelId, imageUploadRequest.File);
        return Ok(new { imageUrl = url });
    }

    /// <summary>
    /// Return list of gallery image for a hotel
    /// </summary>
    /// <param name="hotelId"></param>
    /// <returns>The hotel gallery</returns>
    [HttpGet("{hotelId:guid}/gallery")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ImageResponse>>> GetHotelGallery([FromRoute]Guid hotelId)
    {
        var gallery = await hotelService.GetHotelGallery(hotelId);
        
        var galleryResponse = galleryImageMapper.MapGalleryImageToResponse(gallery);
        return Ok(galleryResponse);
    }

    /// <summary>
    ///   Return specific N hotel featured deals.
    /// </summary>
    /// <param name="listCount">determines the featured deal hotels list size</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The requested number of hotel featured deals.</returns>
    /// <response code="200">Returns the requested number of hotel featured deals.</response>
    /// <response code="400">If the count is less than 1 or greater than 100.</response>
    [HttpGet("featured-deals")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<List<HotelFeaturedDealResponse>> GetFeaturedDealsHotels(int listCount, CancellationToken cancellationToken = default)
    {
        
        var roomInfos =  await hotelService.GetTopFeaturedDealsHotels(listCount, cancellationToken);
        var featuredDeals = roomInfos
            .Select(hotelResponseMapper.MapWithDiscount)
            .ToList();
        return featuredDeals;
    }
}

