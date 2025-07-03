using Sieve.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Filtering.Interfaces;

public interface ISieveProcessorWrapper
{
    IQueryable<TEntity> Apply<TEntity>(SieveModel model,
        IQueryable<TEntity> source,
        bool applyFiltering = true ,
        bool applySorting = true ,
        object[]? data = null ) ;
}
