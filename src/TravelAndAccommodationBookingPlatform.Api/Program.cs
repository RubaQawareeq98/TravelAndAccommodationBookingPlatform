using CloudinaryDotNet;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using QuestPDF.Infrastructure;
using Serilog;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Api.Configurations;
using TravelAndAccommodationBookingPlatform.Api.Configurations.ElasticSearch;
using TravelAndAccommodationBookingPlatform.Api.Middlewares;
using TravelAndAccommodationBookingPlatform.Application.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Bookings;

namespace TravelAndAccommodationBookingPlatform.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.AddInfrastructureConfigurations(builder.Configuration)
            .AddApplication();
        builder.AddWebConfigurations();

        builder.Services.AddControllers().AddNewtonsoftJson();

                
        var elasticSearchConfig = builder.Configuration
            .GetSection("ElasticSearch")
            .Get<ElasticSearchConfigurations>();
                
        if (elasticSearchConfig is not null)
        {
            builder.Host.AddSerilogWithElasticSearch(elasticSearchConfig);
        }
        else
        {
            builder.Host.UseSerilog((_, lc) => lc
                .WriteTo.Console()
                .Enrich.FromLogContext());
        }

        builder.Services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        builder.AddJwtParams();

        builder.Services.AddScoped<ISieveProcessor, SieveProcessor>();
                
        builder.Services.AddSingleton<Cloudinary>(sp =>
        {
            var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
            var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
            return new Cloudinary(account);
        });



        QuestPDF.Settings.License = LicenseType.Community;


        builder.Services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
            return new Account(config.CloudName, config.ApiKey, config.ApiSecret);
        });
            
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

        builder.Services.AddSwaggerGen();


        var serviceProvider = builder.Services.BuildServiceProvider();
        await BookingService.TestConcurrentBookings(serviceProvider);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.MapControllers();

        await app.RunAsync();
    }
}
