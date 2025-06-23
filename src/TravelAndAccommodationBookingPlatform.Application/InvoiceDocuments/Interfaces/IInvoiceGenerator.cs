using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Application.InvoiceDocuments.Interfaces;

public interface IInvoiceGenerator
{
    byte[] GenerateInvoicePdf(Booking booking);
}
