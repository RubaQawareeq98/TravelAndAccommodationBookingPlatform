using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Application.Interfaces.Emails;

public interface IEmailService
{
    Task SendConfirmationEmail(User user, string hotelName, Booking booking);
}
