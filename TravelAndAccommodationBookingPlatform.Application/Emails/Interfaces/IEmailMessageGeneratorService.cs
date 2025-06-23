using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Application.Interfaces.Emails;

public interface IEmailMessageGeneratorService
{
    string GenerateEmailMessage(string userName, string hotelName, Booking booking);
}
