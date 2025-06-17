using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Validators;

public class GetFeaturedDealsRequestValidator : AbstractValidator<GetFeaturedDealsRequest>
{
    public GetFeaturedDealsRequestValidator()
    {
        RuleFor(fd => fd.ListCount)
            .InclusiveBetween(1, 50)
            .WithMessage("List count must be between 1 and 50")
            .WithErrorCode("InvalidListCount");
    }
}
