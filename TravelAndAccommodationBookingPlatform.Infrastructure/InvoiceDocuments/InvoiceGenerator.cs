using QuestPDF.Fluent;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.InvoiceDocuments;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.InvoiceDocuments;

public class InvoiceGenerator : IInvoiceGenerator
{
    public byte[] GenerateInvoicePdf(Booking booking)
    {
        var document = new InvoiceDocument(booking);
        return document.GeneratePdf();
    }
}
