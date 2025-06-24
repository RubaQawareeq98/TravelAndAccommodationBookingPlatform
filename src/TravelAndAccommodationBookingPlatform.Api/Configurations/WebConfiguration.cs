namespace TravelAndAccommodationBookingPlatform.Api.Configurations;

public static class WebConfiguration
{
    public static IServiceCollection AddWebConfigurations(this WebApplicationBuilder builder)
    {
        return builder.AddServices()
            .AddValidatorsConfigurations()
            .AddMapperConfigurations();
    }
}
