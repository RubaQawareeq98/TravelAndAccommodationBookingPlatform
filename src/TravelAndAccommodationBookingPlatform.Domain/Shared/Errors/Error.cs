using TravelAndAccommodationBookingPlatform.Domain.Shared.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Shared.Errors;

public class Error(string code, string message, ErrorType type)
{
    public string Code { get; } = code;
    public string Message { get; } = message;
    public ErrorType Type { get; } = type;

    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
}
