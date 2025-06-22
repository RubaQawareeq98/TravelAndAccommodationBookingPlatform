namespace TravelAndAccommodationBookingPlatform.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    Task BeginTransaction(CancellationToken cancellationToken = default);
    Task Commit(CancellationToken cancellationToken = default);
    Task Rollback(CancellationToken cancellationToken = default);
    Task<int> SaveChanges(CancellationToken cancellationToken = default);
}
