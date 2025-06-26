using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Images.Dtos.Response;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Images.Mappers;

[Mapper]
public partial class GalleryImageMapper
{
    public partial List<ImageResponse> MapGalleryImageToResponse(List<GalleryImage> galleryImages);
}
