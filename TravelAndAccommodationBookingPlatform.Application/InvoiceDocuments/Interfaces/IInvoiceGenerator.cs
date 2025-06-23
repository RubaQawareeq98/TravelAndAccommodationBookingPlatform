using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Application.Interfaces.InvoiceDocuments;

public interface IInvoiceGenerator
{
    byte[] GenerateInvoicePdf(Booking booking);
}
