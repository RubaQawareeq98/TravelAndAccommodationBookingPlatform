using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.EntityConfigurations;

public class RoomCategoryConfiguration : IEntityTypeConfiguration<RoomCategory>
{
    public void Configure(EntityTypeBuilder<RoomCategory> builder)
    {
        builder.HasKey(ri => ri.Id);
        
        builder.Property(ri => ri.RoomType)
            .HasConversion(new EnumToStringConverter<RoomType>());
        
        builder.HasMany(ri => ri.Rooms)
            .WithOne(r => r.RoomCategory)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(r => r.IsDeleted);
    }
}
