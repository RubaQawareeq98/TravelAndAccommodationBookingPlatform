using CloudinaryDotNet.Actions;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Images.CloudinaryService.Interfaces;

public interface ICloudinaryWrapper
{
    Task<ImageUploadResult> UploadAsync(ImageUploadParams uploadParams);
}
