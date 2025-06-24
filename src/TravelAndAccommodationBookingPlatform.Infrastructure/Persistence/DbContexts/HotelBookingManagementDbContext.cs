using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.EntityConfigurations;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

public class HotelBookingManagementDbContext(DbContextOptions<HotelBookingManagementDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public virtual DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomCategory> RoomCategories { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<City> Cities { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<PaymentDetail> PaymentDetails { get; set; }
    public DbSet<GalleryImage> GalleryImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AmenityConfiguration());
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountConfiguration());
        modelBuilder.ApplyConfiguration(new HotelConfiguration());
        modelBuilder.ApplyConfiguration(new ImageConfiguration());
        modelBuilder.ApplyConfiguration(new OwnerConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentDetailConfiguration());
        modelBuilder.ApplyConfiguration(new RoomCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        var owner1Id = Guid.Parse("2a5294a7-202d-4473-84d0-3f8c2cddfac7");
        var owner2Id = Guid.Parse("2a5294a7-202d-4473-84d0-3f8c2cddfac8");
        var city1Id = Guid.Parse("71a53161-8f7a-4ebc-87c2-87c29e5be4b4");
        var city2Id = Guid.Parse("71a53161-8f7a-4ebc-87c2-87c29e5be4b5");
        var hotel1Id = Guid.Parse("85e91235-6799-4e62-a35b-920601e1a9db");
        var hotel2Id = Guid.Parse("85e91235-6799-4e62-a35b-920601e1a9dc");
        var roomInfo1Id = Guid.Parse("85e91235-6799-4e63-a35b-920601e1a9db");
        var roomInfo2Id = Guid.Parse("85e91235-6799-4e64-a35b-920601e1a9dc");
        var room1Id = Guid.Parse("85e91235-6799-4e62-a36b-920601e1a9db");
        var room2Id = Guid.Parse("85e91235-6799-4e62-a37b-920601e1a9dc");
        var discount1Id = Guid.Parse("85e91235-6799-4e63-a35b-920601e1a9dc");

        // Seed Owners
        modelBuilder.Entity<Owner>().HasData(
            new Owner
            {
                Id = owner1Id,
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "0799999999"
            },
            new Owner
            {
                Id = owner2Id,
                Email = "alice.smith@example.com",
                FirstName = "Alice",
                LastName = "Smith",
                PhoneNumber = "0788888888"
            }
        );

        // Seed Cities
        modelBuilder.Entity<City>().HasData(
            new City
            {
                Id = city1Id,
                Name = "Amman",
                Country = "Jordan",
                PostalCode = "11118"
            },
            new City
            {
                Id = city2Id,
                Name = "Istanbul",
                Country = "Turkey",
                PostalCode = "34000"
            }
        );

        // Seed Hotels
        modelBuilder.Entity<Hotel>().HasData(
            new Hotel
            {
                Id = hotel1Id,
                Name = "Luxury Stay Amman",
                Description = "A luxurious hotel in the heart of Amman.",
                PhoneNumber = "0799999999",
                StarRating = 5,
                Longitude = 35.9128,
                Latitude = 31.9539,
                TotalRooms = 100,
                HotelType = HotelType.Luxury,
                CityId = city1Id,
                OwnerId = owner1Id
            },
            new Hotel
            {
                Id = hotel2Id,
                Name = "Sea Breeze Istanbul",
                Description = "Charming seaside accommodation in Istanbul.",
                PhoneNumber = "0788888888",
                StarRating = 4,
                Longitude = 28.9784,
                Latitude = 41.0082,
                TotalRooms = 80,
                HotelType = HotelType.Business,
                CityId = city2Id,
                OwnerId = owner2Id
            }
        );

    }
}