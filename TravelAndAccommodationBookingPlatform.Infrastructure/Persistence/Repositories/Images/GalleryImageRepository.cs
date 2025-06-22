using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Images;

public class GalleryImageRepository(HotelBookingManagementDbContext dbContext) : IGalleryImageRepository
{
    public async Task AddImage(GalleryImage galleryImage)
    {
        await dbContext.GalleryImages.AddAsync(galleryImage);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<List<GalleryImage>> GetAllImagesByEntityId(Guid entityId)
    {
        return await dbContext.GalleryImages
            .Where(x => x.EntityId == entityId)
            .ToListAsync();
    }
}
