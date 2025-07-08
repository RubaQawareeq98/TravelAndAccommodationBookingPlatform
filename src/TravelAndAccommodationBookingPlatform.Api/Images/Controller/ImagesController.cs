using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.Api.Extensions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Images.Controller;

[Route("api/images")]
[Authorize(Roles = "Admin")]
[ApiController]
public class ImagesController(IGalleryImageService galleryImageService) : ControllerBase
{
    /// <summary>
    /// Remove image by image id
    /// </summary>
    /// <param name="imageId">ID of image to be removed</param>
    /// <returns></returns>
    [HttpDelete("{imageId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteImage([FromRoute] Guid imageId)
    {
        var result = await galleryImageService.DeleteGalleryImage(imageId);
        return result.ToActionResult();
    }
}
