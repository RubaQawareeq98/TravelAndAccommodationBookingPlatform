using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Infrastructure.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.Emails.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.Payments.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Api.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<CloudinarySettings>(
            builder.Configuration.GetSection("CloudinarySettings"));

        builder.Services.Configure<BrevoSettings>(
            builder.Configuration.GetSection("BrevoSettings"));
        
        builder.Services.Configure<JwtAuthOptions>(
            builder.Configuration.GetSection("JwtAuthentication"));

        builder.Services.Configure<StripeSettings>(
            builder.Configuration.GetSection("Stripe"));
        
        builder.Services.AddDbContext<HotelBookingManagementDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnectionString")));
        
        return builder.Services;
    }
}
