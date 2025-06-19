using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Application.Features.RecentlyVisitedHotels.Dtos;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Mappers;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Bookings;

public class BookingRepository(HotelBookingManagementDbContext dbContext,
    ISieveProcessor sieveProcessor,
    ILogger<BookingRepository> logger)
    : IBookingRepository
{
    public async Task AddBooking(Booking booking, List<Room> rooms)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(
                IsolationLevel.ReadCommitted);
        
            try
            {
                foreach (var roomId in rooms.Select(r => r.Id))
                {
                    var trackedRoom = await dbContext.Rooms.FindAsync(roomId);
                    if (trackedRoom is null)
                    {
                        throw new InvalidDataException($"Room with ID {roomId} not found.");
                    }
                    trackedRoom.UpdatedAt = DateTime.UtcNow;
                    booking.Rooms.Add(trackedRoom);
                }
        
                dbContext.Bookings.Add(booking);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                logger.LogWarning(ex, "Concurrency conflict booking rooms: {Message}", ex.Message);
                throw new DbUpdateConcurrencyException();
            }
            catch (DbUpdateException ex) when (IsDeadlock(ex))
            {
                logger.LogWarning(ex, "Deadlock occurred during booking: {Message}", ex.Message);
                throw new DbUpdateException();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error creating booking");
                throw new InvalidOperationException("message", ex);
            }
        });
    }
    
    private static bool IsDeadlock(Exception ex)
    {
        return ex.InnerException is SqlException { Number: 1205 };
    }

    public async Task UpdateBooking(Booking booking)
    {
        dbContext.Bookings.Update(booking);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteBooking(Booking booking)
    {
        dbContext.Bookings.Remove(booking);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Booking?> GetBooking(Guid id)
    {
        return await dbContext.Bookings
            .Include(b => b.PaymentDetail)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Booking>> GetAllBookings(SieveModel sieveModel)
    {
        var query = dbContext.Bookings
            .Include(b => b.PaymentDetail)
            .AsQueryable();
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync();
    }

    public async Task<List<Booking>> GetUserRecentlyVisitedHotels(Guid userId, int listCount,
        CancellationToken cancellationToken = default)
    {
        const string query = """
                                 SELECT h.Name As HotelName, h.ThumbnailUrl, h.StarRating,
                                     c.Name AS CityName, c.Country AS CountryName, c.PostalCode,
                                     p.Amount AS Price, p.PaymentMethod,
                                     t.CheckInDate, t.HotelId, t.CheckOutDate
                                 FROM (
                                     SELECT *
                                     FROM (
                                         SELECT
                                             b.Id,
                                             b.HotelId,
                                             b.UserId,
                                             b.CheckInDate,
                                             b.CheckOutDate,
                                             ROW_NUMBER() OVER (PARTITION BY b.HotelId ORDER BY b.CheckInDate DESC) AS rn
                                         FROM Bookings b
                                         WHERE b.UserId = @userId
                                     ) t
                                     WHERE t.rn = 1
                                 ) AS t
                                 INNER JOIN Hotels h ON t.HotelId = h.Id
                                 INNER JOIN Cities c ON h.CityId = c.Id
                                 INNER JOIN PaymentDetails p ON t.Id = p.BookingId
                                 ORDER BY t.CheckInDate DESC
                                 OFFSET 0 ROWS FETCH NEXT @listCount ROWS ONLY
                             """;
        
        var result = await dbContext.Database
            .SqlQueryRaw<RecentlyVisitedDto>(query, new SqlParameter("@userId", userId),
                new SqlParameter("@listCount", listCount))
            .ToListAsync(cancellationToken);

        var bookings = result.MapToBookings();
        return bookings;
    }
}
