using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Infrastructure.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Api.Configurations;
using TravelAndAccommodationBookingPlatform.Api.Middlewares;
using TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddInfrastructureConfigurations(builder.Configuration)
    .AddWebConfigurations();

builder.Services.AddControllers().AddNewtonsoftJson();



builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddScoped<ISieveProcessor, SieveProcessor>();

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    return new Account(config.CloudName, config.ApiKey, config.ApiSecret);
});

builder.Services.Configure<JwtAuthOptions>(
    builder.Configuration.GetSection("JwtAuthentication"));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDbContext<HotelBookingManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnectionString")));

builder.Services.AddSwaggerGen();




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