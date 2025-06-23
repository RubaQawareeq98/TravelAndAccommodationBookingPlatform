using QuestPDF.Fluent;
using TravelAndAccommodationBookingPlatform.Application.InvoiceDocuments.Interfaces;
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
