using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Infrastructure.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Validators;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Cities.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Cities.Validators;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Images.Dtos;
using TravelAndAccommodationBookingPlatform.Api.Images.Validators;
using TravelAndAccommodationBookingPlatform.Api.Middlewares;
using TravelAndAccommodationBookingPlatform.Api.Owners.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Owners.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Owners.Validators;
using TravelAndAccommodationBookingPlatform.Api.RoomInfos.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.RoomInfos.Mappers;
using TravelAndAccommodationBookingPlatform.Api.RoomInfos.Validators;
using TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth;
using TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddInfrastructureConfigurations(builder.Configuration);

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddSingleton<IValidator<LoginRequest>, LoginRequestValidator>();
builder.Services.AddSingleton<IValidator<RegisterRequest>, RegisterRequestValidator>();
builder.Services.AddSingleton<IValidator<AddCityRequest>, AddCityRequestValidator>();
builder.Services.AddSingleton<IValidator<AddOwnerRequest>, AddOwnerRequestValidator>();
builder.Services.AddSingleton<IValidator<UpdateOwnerRequest>, UpdateOwnerRequestValidator>();
builder.Services.AddSingleton<IValidator<UpdateCityRequest>, UpdateCityRequestValidator>();
builder.Services.AddSingleton<IValidator<ThumbnailImageUploadRequest>, ThumbnailImageUploadRequestValidator>();
builder.Services.AddSingleton<IValidator<AddRoomInfoRequest>, AddRoomInfoRequestValidator>();
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddSingleton<RegisterRequestMapper>();
builder.Services.AddSingleton<CityRequestMapper>();
builder.Services.AddSingleton<HotelRequestMapper>();
builder.Services.AddSingleton<OwnerRequestMapper>();
builder.Services.AddSingleton<RoomInfoRequestMapper>();
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