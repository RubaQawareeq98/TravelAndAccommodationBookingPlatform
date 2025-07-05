using brevo_csharp.Api;
using brevo_csharp.Model;
using ITransactionalEmailsApi = TravelAndAccommodationBookingPlatform.Infrastructure.Emails.Interfaces.ITransactionalEmailsApi;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Emails.Services;

public class TransactionalEmailsApiAdapter : ITransactionalEmailsApi
{
    private readonly TransactionalEmailsApi _api = new();

    public async Task<CreateSmtpEmail> SendTransacEmail(SendSmtpEmail email)
    {
        return await _api.SendTransacEmailAsync(email);
    }
}
