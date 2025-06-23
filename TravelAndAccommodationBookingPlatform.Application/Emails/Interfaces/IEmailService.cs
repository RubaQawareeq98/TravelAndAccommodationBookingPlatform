using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Application.Emails.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmail(User user, string hotelName, Booking booking, byte[] invoicePdf);
}
