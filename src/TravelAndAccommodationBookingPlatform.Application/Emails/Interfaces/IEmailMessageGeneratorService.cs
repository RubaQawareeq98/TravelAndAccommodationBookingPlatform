using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Application.Emails.Interfaces;

public interface IEmailMessageGeneratorService
{
    string GenerateEmailMessage(string userName, string hotelName, Booking booking);
}
