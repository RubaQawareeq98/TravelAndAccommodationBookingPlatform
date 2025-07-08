using AutoFixture;
using FluentAssertions;
using Moq;
using Sieve.Models;
using TAABP.UnitTests.Shared;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Owners;

namespace TAABP.UnitTests.Owners.Repositories;

public class OwnerRepositoryUnitTests : RepositoryUnitTestBase<HotelBookingManagementDbContext, Owner>
{
    private readonly OwnerRepository _ownerRepository;

    public OwnerRepositoryUnitTests()
    {
        var owners = Fixture.CreateMany<Owner>(3).ToList();
        SetupMockDbSet(owners, ctx => ctx.Owners);
        SetupSieveProcessor();

        Fixture.Register(() =>
            new OwnerRepository(MockDbContext.Object, MockSieveProcessorWrapper.Object, MockUnitOfWork.Object));
        
        _ownerRepository = Fixture.Create<OwnerRepository>();
    }

    [Fact]
    public async Task GetAllOwners_ShouldReturnAllOwners()
    {
        // Arrange 
        var sieveModel = Fixture.Create<SieveModel>();

        // Act
        var result = await _ownerRepository.GetOwners(sieveModel);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task AddOwner_ShouldAddAndSaveChanges()
    {
        // Arrange
        var owner = Fixture.Create<Owner>();

        // Act
        await _ownerRepository.AddOwner(owner);

        // Assert
        MockDbSet.Verify(d => d.AddAsync(owner, It.IsAny<CancellationToken>()), Times.Once);
        MockUnitOfWork.Verify(u => u.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateOwner_ShouldUpdateAndSaveChanges()
    {
        // Arrange
        var owner = Fixture.Create<Owner>();

        // Act
        await _ownerRepository.UpdateOwner(owner);

        // Assert
        MockDbSet.Verify(d => d.Update(owner), Times.Once);
        MockUnitOfWork.Verify(u => u.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteOwner_ShouldSoftDeleteAndSaveChanges()
    {
        // Arrange
        var owner = Fixture.Create<Owner>();

        // Act
        await _ownerRepository.DeleteOwner(owner);

        // Assert
        owner.IsDeleted.Should().BeTrue();
        MockUnitOfWork.Verify(u => u.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetOwner_ShouldReturnOwner_WhenExistsAndNotDeleted()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var owners = new List<Owner>
        {
            Fixture.Build<Owner>().With(x => x.Id, ownerId).With(x => x.IsDeleted, false).Create(),
            Fixture.Create<Owner>()
        };

        SetupMockDbSet(owners, ctx => ctx.Owners);

        // Act
        var result = await _ownerRepository.GetOwner(ownerId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(ownerId);
    }

    [Fact]
    public async Task GetOwner_ShouldReturnNull_WhenOwnerDoesNotExist()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var owners = Fixture.CreateMany<Owner>(3).ToList();
        SetupMockDbSet(owners, ctx => ctx.Owners);

        // Act
        var result = await _ownerRepository.GetOwner(ownerId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetOwner_ShouldReturnNull_WhenOwnerIsDeleted()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var deletedOwner = Fixture.Build<Owner>()
            .With(x => x.Id, ownerId)
            .With(x => x.IsDeleted, true)
            .Create();

        var owners = new List<Owner> { deletedOwner };
        SetupMockDbSet(owners, ctx => ctx.Owners);

        // Act
        var result = await _ownerRepository.GetOwner(ownerId);

        // Assert
        result.Should().BeNull();
    }
}
