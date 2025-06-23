using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.EntityConfigurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasKey(h => h.Id);
        
        builder.HasMany(h => h.Bookings)
            .WithOne(b => b.Hotel)
            .HasForeignKey(h => h.HotelId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(h => h.RoomCategories)
            .WithOne(r => r.Hotel)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(h => h.HotelType)
            .HasConversion(new EnumToStringConverter<HotelType>());
        
        builder.Property(h => h.Longitude)
            .HasPrecision(8, 6);

        builder.Property(h => h.Latitude)
            .HasPrecision(8, 6);
        
        builder.Ignore(h => h.Gallery);
    }
}
