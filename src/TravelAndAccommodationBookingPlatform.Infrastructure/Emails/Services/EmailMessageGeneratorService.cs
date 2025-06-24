using TravelAndAccommodationBookingPlatform.Application.Emails.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Emails.Services;

public class EmailMessageGeneratorService : IEmailMessageGeneratorService
{
    public string GenerateEmailMessage(string userName, string hotelName, Booking booking)
    {
        return $"""
                    <html>
                    <body style="font-family: Arial, sans-serif; line-height: 1.6;">
                        <h2 style="color: #4CAF50;">Hello {userName},</h2>
                        <p>Thank you for your reservation! Here are your booking details:</p>
                
                        <table style="border-collapse: collapse;">
                            <tr>
                                <td style="padding: 8px;"><strong>Hotel:</strong></td>
                                <td style="padding: 8px;">{hotelName}</td>
                            </tr>
                            <tr>
                                <td style="padding: 8px;"><strong>Check-in Date:</strong></td>
                                <td style="padding: 8px;">{booking.CheckInDate:MMMM dd, yyyy}</td>
                            </tr>
                            <tr>
                                <td style="padding: 8px;"><strong>Check-out Date:</strong></td>
                                <td style="padding: 8px;">{booking.CheckOutDate:MMMM dd, yyyy}</td>
                            </tr>
                            <tr>
                                <td style="padding: 8px;"><strong>Total Amount:</strong></td>
                                <td style="padding: 8px;">${booking.PaymentDetail.Amount:F2}</td>
                            </tr>
                        </table>
                
                        <p>If you have any questions or need to make changes to your booking, feel free to contact us.</p>
                        <p>We look forward to hosting you!</p>
                
                        <br />
                        <p style="color: #888;">Best regards,<br/>The Hotel Booking Team</p>
                    </body>
                    </html>
                """;
    }
}
