using Microsoft.AspNetCore.Http;
using TravelAndAccommodationBookingPlatform.Application.Images.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Images;

public class GalleryImageService(IGalleryImageRepository galleryImageRepository,
    IImageService imageService) : IGalleryImageService
{
    public async Task<List<GalleryImage>> GetAllImagesByEntityId(Guid entityId)
    {
        return await galleryImageRepository.GetAllImagesByEntityId(entityId);
    }

    public async Task<string> AddGalleryImage(Guid entityId, IFormFile file)
    {
        var path = await imageService.UploadImageAsync(file);

        var image = new GalleryImage
        {
            EntityId = entityId,
            Path = path,
        };
        await galleryImageRepository.AddImage(image);
        
        return path;  
    }

    public async Task<Result> DeleteGalleryImage(Guid imageId)
    {
        var image = await galleryImageRepository.GetImageById(imageId);
        if (image is null)
        {
            return Result.Failure(ImageError.ImageNotFound(imageId));
        }
        
        await galleryImageRepository.DeleteImage(image);
        return Result.Success();
    }
}
