using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using QuestPDF.Infrastructure;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.InvoiceDocuments;

namespace TAABP.UnitTests.InvoiceDocuments;

public class InvoiceGeneratorTests
{
    private readonly IFixture _fixture;

    public InvoiceGeneratorTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void GenerateInvoicePdf_ShouldReturnNonEmptyPdfBytes()
    {
        // Arrange
        QuestPDF.Settings.License = LicenseType.Community;
        var booking = _fixture.Create<Booking>();
        var generator = new InvoiceGenerator();

        // Act
        var pdfBytes = generator.GenerateInvoicePdf(booking);

        // Assert
        pdfBytes.Should().NotBeNull();
        pdfBytes.Length.Should().BeGreaterThan(100);
    }
}
