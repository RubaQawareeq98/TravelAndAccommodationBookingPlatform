using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using TravelAndAccommodationBookingPlatform.Application.Images.Interfaces;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Images;

public class ImageService(Account account) : IImageService
{
    private readonly Cloudinary _cloudinary = new(account);

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "uploads"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult.SecureUrl.ToString();
    }
}
