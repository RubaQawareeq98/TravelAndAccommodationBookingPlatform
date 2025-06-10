using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.EntityConfigurations;

public class PaymentDetailConfiguration : IEntityTypeConfiguration<PaymentDetail>
{
    public void Configure(EntityTypeBuilder<PaymentDetail> builder)
    {
        builder.HasKey(pd => pd.BookingId);
    }
}
