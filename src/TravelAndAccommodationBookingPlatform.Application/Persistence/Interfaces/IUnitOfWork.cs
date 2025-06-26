using System.Data;

namespace TravelAndAccommodationBookingPlatform.Application.Persistence.Interfaces;

public interface IUnitOfWork
{
    Task BeginTransaction(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
    Task Commit(CancellationToken cancellationToken = default);
    Task Rollback(CancellationToken cancellationToken = default);
    Task<int> SaveChanges(CancellationToken cancellationToken = default);
}
