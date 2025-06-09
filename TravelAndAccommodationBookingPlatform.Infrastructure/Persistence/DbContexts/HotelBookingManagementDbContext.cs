using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

public class HotelBookingManagementDbContext (DbContextOptions<HotelBookingManagementDbContext> options) : DbContext (options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomInfo> RoomInfos { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<PaymentDetail> PaymentDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentDetail>()
            .HasKey(p => p.BookingId); 
    }
}
