using Stripe;
using TravelAndAccommodationBookingPlatform.Application.Payments.Dtos;
using TravelAndAccommodationBookingPlatform.Application.Payments.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Payments.Services;

public class StripePaymentService : IPaymentService
{
    
    public StripePaymentService()
    {
        StripeConfiguration.ApiKey = "sk_test_51Rd5nnCXZS4CbJzkxDZMnpfideorWNVBg0nf2SIjR4ldQsbUOIas2GPmzNzaV3Jgbcoj9SUjbb06jfn7Z6xsb9dx00B3DWp6ec";
    }

    public async Task<Result<string>> CreatePaymentService(AddPaymentRequest request)
    {
        var paymentIntentService = new PaymentIntentService();
        var stripeAmount = (long)(request.Amount * 100);
        
        var options = new PaymentIntentCreateOptions
        {
            Amount = stripeAmount,
            Currency = request.Currency,
            ReceiptEmail = request.ReceiptEmail,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        };

        try
        {
            var paymentIntent = await paymentIntentService.CreateAsync(options);
            return Result<string>.Success(paymentIntent.ClientSecret);
        }
        catch (StripeException ex)
        {
            throw new StripeException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
}
