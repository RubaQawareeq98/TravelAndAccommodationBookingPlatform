using Microsoft.AspNetCore.Http;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Images;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

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
}
