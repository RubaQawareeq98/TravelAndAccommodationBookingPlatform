using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.EntityConfigurations;

public class RoomInfoConfiguration : IEntityTypeConfiguration<RoomInfo>
{
    public void Configure(EntityTypeBuilder<RoomInfo> builder)
    {
        builder.HasKey(ri => ri.Id);
        
        builder.Property(ri => ri.RoomType)
            .HasConversion(new EnumToStringConverter<RoomType>());
        
        builder.HasMany(ri => ri.Rooms)
            .WithOne(r => r.RoomInfo)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
