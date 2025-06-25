using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using TravelAndAccommodationBookingPlatform.Infrastructure.Images.CloudinaryService.Interfaces;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Images.CloudinaryService;

public class CloudinaryWrapper(Cloudinary cloudinary) : ICloudinaryWrapper
{

    public async Task<ImageUploadResult> UploadAsync(ImageUploadParams uploadParams)
    {
        return await cloudinary.UploadAsync(uploadParams);
    }
}
