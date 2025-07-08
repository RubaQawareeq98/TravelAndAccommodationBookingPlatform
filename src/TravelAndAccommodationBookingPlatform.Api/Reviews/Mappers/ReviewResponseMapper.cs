using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Reviews.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Reviews.Mappers;

[Mapper]
public partial class ReviewResponseMapper
{
    public partial ReviewResponse MapReviewToReviewResponse(Review review);
    public partial List<ReviewResponse>  MapReviewListToReviewResponseList(List<Review> reviews);
}
