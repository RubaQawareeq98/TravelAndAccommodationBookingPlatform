using CloudinaryDotNet;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using QuestPDF.Infrastructure;
using Serilog;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Api.Configurations;
using TravelAndAccommodationBookingPlatform.Api.Configurations.ElasticSearch;
using TravelAndAccommodationBookingPlatform.Api.Middlewares;
using TravelAndAccommodationBookingPlatform.Application.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth.Configurations;

namespace TravelAndAccommodationBookingPlatform.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        
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
        
        QuestPDF.Settings.License = LicenseType.Community;
        
        builder.AddInfrastructureConfigurations(builder.Configuration)
            .AddApplication();
        builder.AddWebConfigurations();
        
        builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
        
        builder.Services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();
        
        builder.Services.AddScoped<ISieveProcessor, SieveProcessor>();
        
        builder.Services.AddSingleton<Cloudinary>(sp =>
        {
            var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
            var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
            return new Cloudinary(account);
        });
        
        builder.Services.AddSwaggerGen();

        builder.AddJwtParams();
        
        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        Console.WriteLine("Ruba");
        app.Use(async (context, next) =>
        {
            Console.WriteLine("Hello World!");
            var authHeader = context.Request.Headers.Authorization.ToString();
            Console.WriteLine("Authorization Header: " + authHeader); // or use logger
            await next();
        });
        
        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
   

        
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.UseSerilogRequestLogging();
        
        app.UseMiddleware<RequestLoggingMiddleware>(); 
        
        app.MapControllers();
        
        await app.RunAsync();
    }
}