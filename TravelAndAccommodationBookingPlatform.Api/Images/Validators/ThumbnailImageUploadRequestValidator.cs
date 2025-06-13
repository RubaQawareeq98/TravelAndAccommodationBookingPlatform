using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Images.Dtos;

namespace TravelAndAccommodationBookingPlatform.Api.Images.Validators;

public class ThumbnailImageUploadRequestValidator : AbstractValidator<ThumbnailImageUploadRequest>
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
