using System.Data;
using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Application.Persistence.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Abstraction;

public class UnitOfWork(HotelBookingManagementDbContext dbContext) : IUnitOfWork
{
    public async Task BeginTransaction(CancellationToken cancellationToken = default)
    {
        if (dbContext.Database.CurrentTransaction is null)
        { 
            await dbContext.Database.BeginTransactionAsync(IsolationLevel.Snapshot, cancellationToken);
        }
    }

    public async Task Commit(CancellationToken cancellationToken = default)
    {
        if(dbContext.Database.CurrentTransaction is null)
        {
            return;
        }

        await dbContext.Database.CurrentTransaction.CommitAsync(cancellationToken);
    }

    public async Task Rollback(CancellationToken cancellationToken = default)
    {
        if(dbContext.Database.CurrentTransaction is null)
        {
            return;
        }

        await dbContext.Database.CurrentTransaction.RollbackAsync(cancellationToken);
    }

    public async Task<int> SaveChanges(CancellationToken cancellationToken = default)
    {
        SetAuditInfo();
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private void SetAuditInfo()
    {
        var now = DateTime.UtcNow;
        var entries = dbContext.ChangeTracker.Entries()
            .Where(e => e is { Entity: AuditableSoftDeleteBaseEntity, State: EntityState.Added or EntityState.Modified });

        foreach (var entry in entries)
        {
            var entity = (AuditableSoftDeleteBaseEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = now;
            }
            entity.UpdatedAt = now;
        }
    }
}
