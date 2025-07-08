using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Filtering.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Persistence.Interfaces;

namespace TAABP.UnitTests.Shared;

public abstract class RepositoryUnitTestBase<TDbContext, TEntity> 
    where TDbContext : class
    where TEntity : class
{
    protected readonly IFixture Fixture;
    protected readonly Mock<TDbContext> MockDbContext;
    protected readonly Mock<IUnitOfWork> MockUnitOfWork;
    protected readonly Mock<ISieveProcessorWrapper> MockSieveProcessorWrapper;
    protected Mock<DbSet<TEntity>> MockDbSet;

    protected RepositoryUnitTestBase()
    {
        Fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
        Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => Fixture.Behaviors.Remove(b));
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        MockDbContext = Fixture.Freeze<Mock<TDbContext>>();
        MockUnitOfWork = Fixture.Freeze<Mock<IUnitOfWork>>();
        MockSieveProcessorWrapper = Fixture.Freeze<Mock<ISieveProcessorWrapper>>();
    }

    protected void SetupMockDbSet(List<TEntity> data, Expression<Func<TDbContext, DbSet<TEntity>>> dbSetSelector)
    {
        MockDbSet = data.AsQueryable().CreateMockDbSet();
        MockDbContext.Setup(dbSetSelector).Returns(MockDbSet.Object);
    }


    protected void SetupSieveProcessor()
    {
        MockSieveProcessorWrapper
            .Setup(x => x.Apply(
                It.IsAny<SieveModel>(),
                It.IsAny<IQueryable<TEntity>>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<object[]>()))
            .Returns((SieveModel _, IQueryable<TEntity> q, bool _, bool _, object[]? _) => q);
    }
}
