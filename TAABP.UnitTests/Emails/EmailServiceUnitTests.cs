using AutoFixture;
using AutoFixture.AutoMoq;
using brevo_csharp.Model;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TravelAndAccommodationBookingPlatform.Application.Emails.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Emails.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.Emails.Interfaces;
using TravelAndAccommodationBookingPlatform.Infrastructure.Emails.Services;
using Task = System.Threading.Tasks.Task;

namespace TAABP.UnitTests.Emails;

public class EmailServiceUnitTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IEmailMessageGeneratorService> _mockEmailMessageGeneratorService;
    private readonly Mock<ITransactionalEmailsApi> _mockEmailApi;
    private readonly EmailService _emailService;

    public EmailServiceUnitTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _fixture.Freeze<Mock<ILogger<EmailService>>>();
        _mockEmailMessageGeneratorService = _fixture.Freeze<Mock<IEmailMessageGeneratorService>>();
        _mockEmailApi = _fixture.Freeze<Mock<ITransactionalEmailsApi>>();

        var brevoSettings = _fixture.Create<BrevoSettings>();
        Options.Create(brevoSettings);

        _emailService = _fixture.Create<EmailService>();
    }

    [Fact]
    public async Task SendConfirmationEmail_ShouldSendEmailSuccessfully()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var booking = _fixture.Create<Booking>();
        var pdf = "fake-pdf"u8.ToArray();
        var emailContent = _fixture.Create<string>();
        const string emailSubject = "Booking Confirmation";
        const string attachmentName = "invoice.pdf";

        _mockEmailMessageGeneratorService
            .Setup(m => m.GenerateEmailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Booking>()))
            .Returns(emailContent);

        _mockEmailApi
            .Setup(api => api.SendTransacEmail(It.IsAny<SendSmtpEmail>()))
            .ReturnsAsync(new CreateSmtpEmail());

        // Act
        var act = async () => await _emailService.SendConfirmationEmail(user, booking.Hotel.Name, booking, pdf);

        // Assert
        await act.Should().NotThrowAsync();
        _mockEmailApi.Verify(api => api.SendTransacEmail(It.Is<SendSmtpEmail>(email =>
            email.Subject == emailSubject &&
            email.To.Any(t => t.Email == user.Email) &&
            email.Attachment.Any(a => a.Name == attachmentName)
        )), Times.Once);
    }
}
