using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Rooms.Validators;

public class UpdateRoomRequestValidator : AbstractValidator<UpdateRoomRequest>
{
    public UpdateRoomRequestValidator()
    {
        RuleFor(r => r.RoomNumber)
            .NotEmpty()
            .WithMessage("Room number cannot be empty")
            .WithErrorCode("Invalid Room Number")
            .WithSeverity(Severity.Error)
            .When(r => r.RoomNumber is not null);
    }
}
