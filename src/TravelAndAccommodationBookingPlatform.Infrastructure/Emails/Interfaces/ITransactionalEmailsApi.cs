using brevo_csharp.Model;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Emails.Interfaces;

public interface ITransactionalEmailsApi
{
    Task<CreateSmtpEmail> SendTransacEmail(SendSmtpEmail email);
}
