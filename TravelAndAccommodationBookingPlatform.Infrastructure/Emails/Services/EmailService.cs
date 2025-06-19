using brevo_csharp.Api;
using brevo_csharp.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Emails;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Emails.Configurations;
using Task = System.Threading.Tasks.Task;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Emails.Services;

public class EmailService(ILogger<EmailService> logger,
    IOptions<BrevoSettings> options,
    IEmailMessageGeneratorService emailMessageGeneratorService) : IEmailService
{
    private readonly BrevoSettings _brevoSettings = options.Value;

    public async Task SendConfirmationEmail(User user, string hotelName, Booking booking)
    {
        logger.LogInformation("Sending Confirmation email");
        
        brevo_csharp.Client.Configuration.Default.AddApiKey("api-key", _brevoSettings.ApiKey);

        var userName = $"{user.FirstName} {user.LastName}";
        var htmlContent = emailMessageGeneratorService.GenerateEmailMessage(userName, hotelName, booking);

        var apiInstance = new TransactionalEmailsApi();
        
        var emailSender = new SendSmtpEmailSender(_brevoSettings.SenderName, _brevoSettings.SenderEmail);

        var emailReceiver = new SendSmtpEmailTo(user.Email, user.FirstName);
        
        var receiversList = new List<SendSmtpEmailTo> { emailReceiver };
        try
        {
            var sendSmtpEmail = new SendSmtpEmail(emailSender, receiversList, null, null, htmlContent, null, "Reset Password.");
            
            await apiInstance.SendTransacEmailAsync(sendSmtpEmail);

            logger.LogDebug("Email sent to reset password");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }
}
