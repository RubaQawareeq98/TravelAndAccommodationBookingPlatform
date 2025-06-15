using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Images.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Images.Validators;

public class ThumbnailImageUploadRequestValidator : AbstractValidator<ImageUploadRequest>
{
    public ThumbnailImageUploadRequestValidator()
    {
        RuleFor(i => i.File)
            .NotNull()
            .WithMessage("Please provide a file.")
            .WithErrorCode("File");

        RuleFor(i => i.File.Length)
            .NotNull()
            .WithMessage("Please provide a file length.")
            .GreaterThan(0);
    }
}
