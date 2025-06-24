using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Users.Dtos;

namespace TravelAndAccommodationBookingPlatform.Api.Users.Validators;

public class GetRecentlyVisitedRequestValidator : AbstractValidator<GetRecentlyVisitedRequest>
{
    public GetRecentlyVisitedRequestValidator()
    {
        RuleFor(fd => fd.ListCount)
            .InclusiveBetween(1, 50)
            .WithMessage("List count must be between 1 and 50")
            .WithErrorCode("InvalidListCount");
    }
}
