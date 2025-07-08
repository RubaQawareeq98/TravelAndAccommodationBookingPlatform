using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Reviews.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Reviews.Mappers;

[Mapper]
public partial class ReviewRequestMapper
{
    public partial Review MapAddReviewRequestToReview(AddReviewRequest addReviewRequest);
    public partial void MapUpdateReviewRequestToReview(UpdateReviewRequest updateReviewRequest, Review review);
    public partial UpdateReviewRequest MapReviewToUpdateReviewRequest(Review review);
}
