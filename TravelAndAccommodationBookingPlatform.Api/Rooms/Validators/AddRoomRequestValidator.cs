using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Rooms.Validators;

public class AddRoomRequestValidator : AbstractValidator<AddRoomRequest>
{
    public AddRoomRequestValidator()
    {
        RuleFor(r => r.RoomNumber)
            .NotEmpty()
            .WithMessage("Room number cannot be empty")
            .WithErrorCode("Invalid Room Number")
            .WithSeverity(Severity.Error);
        
        
        RuleFor(r => r.RoomCategoryId)
            .NotEmpty()
            .WithMessage("RoomCategory Id is required")
            .WithErrorCode("InvalidRoomCategory")
            .WithSeverity(Severity.Error);
    }
}
