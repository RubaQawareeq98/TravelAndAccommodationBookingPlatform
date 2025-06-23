using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IGalleryImageRepository
{
    Task AddImage(GalleryImage galleryImage);
    Task<List<GalleryImage>> GetAllImagesByEntityId(Guid entityId);
    Task<GalleryImage?> GetImageById(Guid imageId);
    Task DeleteImage(GalleryImage image);
}
