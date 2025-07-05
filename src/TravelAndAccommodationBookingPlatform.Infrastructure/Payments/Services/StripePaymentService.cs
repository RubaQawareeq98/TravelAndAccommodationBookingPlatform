using Microsoft.Extensions.Options;
using Stripe;
using TravelAndAccommodationBookingPlatform.Application.Payments.Dtos;
using TravelAndAccommodationBookingPlatform.Application.Payments.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;
using TravelAndAccommodationBookingPlatform.Infrastructure.Payments.Configurations;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Payments.Services;

public class StripePaymentService : IPaymentService
{
    private readonly PaymentIntentService _paymentIntentService;

    public StripePaymentService(
        IOptions<StripeSettings> options,
        PaymentIntentService paymentIntentService)
    {
        var stripeSettings = options.Value;
        StripeConfiguration.ApiKey = stripeSettings.ApiKey;
        _paymentIntentService = paymentIntentService;
    }

    public async Task<Result<string>> CreatePaymentService(AddPaymentRequest request)
    {
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
            var paymentIntent = await _paymentIntentService.CreateAsync(options);
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

