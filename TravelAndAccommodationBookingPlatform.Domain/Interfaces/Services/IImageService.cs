using Microsoft.AspNetCore.Http;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface IImageService
{
    Task<string> UploadImageAsync(IFormFile file);
}
