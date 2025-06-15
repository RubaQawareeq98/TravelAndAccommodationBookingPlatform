using Microsoft.AspNetCore.Http;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IGalleryImageService
{
    Task<List<GalleryImage>> GetAllImagesByEntityId(Guid entityId);
    Task<string> AddGalleryImage(Guid entityId, IFormFile file);
}
