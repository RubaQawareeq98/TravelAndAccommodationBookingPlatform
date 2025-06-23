using Microsoft.AspNetCore.Http;

namespace TravelAndAccommodationBookingPlatform.Application.Images.Interfaces;

public interface IImageService
{
    Task<string> UploadImageAsync(IFormFile file);
}
