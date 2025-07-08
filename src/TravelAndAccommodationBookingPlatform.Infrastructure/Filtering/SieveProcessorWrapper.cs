using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Application.Filtering.Interfaces;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Filtering;

public class SieveProcessorWrapper(ISieveProcessor sieveProcessor) : ISieveProcessorWrapper
{
    public IQueryable<TEntity> Apply<TEntity>(SieveModel model,
        IQueryable<TEntity> source,
        bool applyFiltering = true,
        bool applySorting = true,
        object[]? data = null)
    {
        return sieveProcessor.Apply(model, source, data, applyFiltering, applySorting);
    }
}
