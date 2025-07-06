using TravelAndAccommodationBookingPlatform.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.InvoiceDocuments;

public class InvoiceDocument(
    Booking booking)
    : IDocument
{
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);
            page.Size(PageSizes.A4);
            page.PageColor(Colors.White);

            page.Header().Text("Booking Invoice")
                .FontSize(22)
                .Bold()
                .FontColor(Colors.Blue.Medium)
                .AlignCenter();

            page.Content().Column(column =>
            {
                column.Spacing(10);

                column.Item().Text($"Customer: {booking.User.FirstName} {booking.User.LastName}");
                column.Item().Text($"Hotel: {booking.Hotel.Name}");
                column.Item().Text($"Booking Date: {booking.BookingDate:MMMM dd, yyyy}");
                column.Item().Text($"Check-in: {booking.CheckInDate:MMMM dd, yyyy}");
                column.Item().Text($"Check-out: {booking.CheckOutDate:MMMM dd, yyyy}");

                column.Item().PaddingTop(15).Text("Booked Rooms").FontSize(16).Bold();

                column.Item().Table(void (table) =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Room #").Bold();
                        header.Cell().Text("Room Name").Bold();
                        header.Cell().Text("Price/Night").Bold();
                        header.Cell().Text("Discount %").Bold();
                    });

                    foreach (var room in booking.Rooms)
                    {
                        table.Cell().Text(room.RoomNumber);
                        table.Cell().Text(room.RoomCategory.Name);
                        table.Cell().Text($"${room.RoomCategory.PricePerNight:F2}");
                        
                        table.Cell().Text("");
                        table.Cell().Text("");
                    }
                });

                column.Item().PaddingTop(20).AlignRight().Text($"Total Amount: ${booking.PaymentDetails.Amount:F2}")
                    .FontSize(14).Bold();
            });

            page.Footer().AlignCenter().Text("Thank you for booking with us!");
        });
    }
}
