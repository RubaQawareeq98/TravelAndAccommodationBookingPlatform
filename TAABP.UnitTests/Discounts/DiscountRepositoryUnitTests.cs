using AutoFixture;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using TAABP.UnitTests.Shared;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Discounts;

namespace TAABP.UnitTests.Discounts;

public class DiscountRepositoryUnitTests : RepositoryUnitTestBase<HotelBookingManagementDbContext, Discount>
{
    private readonly DiscountRepository _discountRepository;

    public DiscountRepositoryUnitTests()
    {
        var discounts = Fixture.CreateMany<Discount>(3).ToList();
        SetupMockDbSet(discounts, ctx => ctx.Discounts);
        SetupSieveProcessor();

        Fixture.Register(() =>
            new DiscountRepository(MockDbContext.Object,
                MockSieveProcessorWrapper.Object,
                MockUnitOfWork.Object));
        
        _discountRepository = Fixture.Create<DiscountRepository>();
    }
    
    [Fact]
    public async Task AddDiscount_ShouldAddAndSave()
    {
        // Arrange
        var discount = Fixture.Create<Discount>();
        var cancellationToken = CancellationToken.None;

        MockDbSet.Setup(x => x.AddAsync(It.IsAny<Discount>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<EntityEntry<Discount>>());

        MockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken))
            .ReturnsAsync(1);

        // Act
        await _discountRepository.AddDiscount(discount, cancellationToken);

        // Assert
        MockDbSet.Verify(x => x.AddAsync(discount, cancellationToken), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }
    
    [Fact]
    public async Task UpdateDiscount_ShouldCallUpdateAndSave()
    {
        // Arrange
        var discount = Fixture.Create<Discount>();
        var cancellationToken = CancellationToken.None;

        MockDbSet.Setup(x => x.Update(discount))
            .Returns(It.IsAny<EntityEntry<Discount>>());
        MockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken))
            .ReturnsAsync(1);

        // Act
        await _discountRepository.UpdateDiscount(discount);

        // Assert
        MockDbSet.Verify(x => x.Update(discount), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteDiscount_ShouldMarkDeletedAndSave()
    {
        // Arrange
        var discount = Fixture.Create<Discount>();
        var cancellationToken = CancellationToken.None;

        MockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken)).ReturnsAsync(1);

        // Act
        await _discountRepository.DeleteDiscount(discount, cancellationToken);

        // Assert
        MockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }
}
