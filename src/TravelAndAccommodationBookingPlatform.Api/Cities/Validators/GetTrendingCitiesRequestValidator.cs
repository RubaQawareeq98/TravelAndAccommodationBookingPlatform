using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Cities.Validators;

public class GetTrendingCitiesRequestValidator : AbstractValidator<GetTrendingCitiesRequest>
{
    public GetTrendingCitiesRequestValidator()
    {
        RuleFor(c => c.ListCount)
            .NotNull()
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("ListCount must be greater than 0")
            .WithSeverity(Severity.Error);
    }
}
