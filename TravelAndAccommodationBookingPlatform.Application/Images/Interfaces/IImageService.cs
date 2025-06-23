using Microsoft.AspNetCore.Http;

namespace TravelAndAccommodationBookingPlatform.Application.Interfaces.Images;

public interface IImageService
{
    Task<string> UploadImageAsync(IFormFile file);
}
