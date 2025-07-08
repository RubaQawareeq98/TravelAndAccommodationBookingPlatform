using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using TravelAndAccommodationBookingPlatform.Application.Images.Interfaces;
using TravelAndAccommodationBookingPlatform.Infrastructure.Images.CloudinaryService.Interfaces;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Images;

public class ImageService(ICloudinaryWrapper cloudinaryWrapper) : IImageService
{
    public async Task<string> UploadImageAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "uploads"
        };

        var uploadResult = await cloudinaryWrapper.UploadAsync(uploadParams);
        return uploadResult.SecureUrl.ToString();
    }
}
