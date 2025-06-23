namespace TravelAndAccommodationBookingPlatform.Application.Interfaces.Payments.Dtos;

public class AddPaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "usd";
    public string ReceiptEmail { get; set; } = string.Empty;
}
