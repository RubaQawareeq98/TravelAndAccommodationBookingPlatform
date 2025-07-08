using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Features.RecentlyVisitedHotels.Dtos;
using TravelAndAccommodationBookingPlatform.Application.Filtering.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Persistence.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Mappers;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Bookings;

public class BookingRepository(HotelBookingManagementDbContext dbContext,
    ISieveProcessorWrapper sieveProcessor,
    IDiscountRepository discountRepository,
    IUnitOfWork unitOfWork,
    ILogger<BookingRepository> logger)
    : IBookingRepository
{
    public async Task<Booking> AddBooking(Booking booking, List<Room> rooms, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted, cancellationToken);

            try
            {
                foreach (var room in rooms)
                {
                    dbContext.Entry(room).State = EntityState.Unchanged;
                    dbContext.Entry(room.RoomCategory).State = EntityState.Unchanged;
                    room.UpdatedAt = DateTime.UtcNow;
                    booking.Rooms.Add(room);
                }

                var totalAmount = await CalculateTotalAmount(rooms, booking.CheckInDate, booking.CheckOutDate);
                booking.PaymentDetails.Amount = totalAmount;
                booking.BookingDate = DateTime.UtcNow;
                booking.PaymentDetails.PaymentDate = booking.BookingDate;

                dbContext.Bookings.Add(booking);
                await unitOfWork.SaveChanges(cancellationToken);
                await unitOfWork.Commit(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                logger.LogWarning(ex, "Concurrency conflict booking rooms: {Message}", ex.Message);
                throw new DbUpdateConcurrencyException("Concurrency conflict booking rooms", ex);
            }
            catch (DbUpdateException ex) when (IsDeadlock(ex))
            {
                logger.LogWarning(ex, "Deadlock occurred during booking: {Message}", ex.Message);
                await unitOfWork.Rollback(cancellationToken);
                throw new DbUpdateException("Deadlock occurred during booking", ex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error creating booking");
                await unitOfWork.Rollback(cancellationToken);
                throw new InvalidOperationException("message", ex);
            }
        });
        return booking;
    }
    
    private static bool IsDeadlock(Exception ex)
    {
        return ex.InnerException is SqlException { Number: 1205 };
    }

    private async Task<decimal> CalculateTotalAmount(List<Room> rooms, DateOnly checkInDate, DateOnly checkOutDate)
    {
        var nights = (checkOutDate.ToDateTime(TimeOnly.MinValue) - checkInDate.ToDateTime(TimeOnly.MinValue)).Days;

        decimal totalAmount = 0;

        foreach (var roomCategory in rooms.Select(r => r.RoomCategory))
        {
            var pricePerNight = roomCategory.PricePerNight;

            var discountPercentage = await discountRepository.GetDiscountAmountByRoomId(roomCategory.Id);
            
            var discountedPrice = pricePerNight * (1 - discountPercentage / 100m);
            var roomTotal = discountedPrice * nights;
            
            totalAmount += roomTotal;
        }

        return totalAmount;
    }
    
    public async Task UpdateBooking(Booking booking)
    {
        dbContext.Bookings.Update(booking);
        await unitOfWork.SaveChanges();
    }

    public async Task DeleteBooking(Booking booking)
    {
        dbContext.Bookings.Remove(booking);
        await unitOfWork.SaveChanges();
    }

    public async Task<Booking?> GetBooking(Guid userId, Guid bookingId, CancellationToken cancellationToken)
    {
        return await dbContext.Bookings
            .Include(b => b.PaymentDetails)
            .AsSplitQuery()
            .FirstOrDefaultAsync(b => b.UserId == userId && b.Id == bookingId, cancellationToken);
    }

    public async Task<Booking?> GetBookingWithDetails(Guid userId, Guid bookingId, CancellationToken cancellationToken)
    {
        return await dbContext.Bookings
            .Where(b => b.UserId == userId && b.Id == bookingId)
            .Select(b => new Booking
            {
                Id = b.Id,
                BookingDate = b.BookingDate,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                GuestRemarks = b.GuestRemarks,
                HotelId = b.HotelId,
                Hotel = new Hotel
                {
                    Name = b.Hotel.Name,
                    Description = b.Hotel.Description
                },
                UserId = b.User.Id,
                User = new User
                {
                    FirstName = b.User.FirstName,
                    LastName = b.User.LastName,
                },
                PaymentDetails = b.PaymentDetails,
                Rooms = b.Rooms.Select(r => new Room
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    RoomCategoryId = r.RoomCategoryId,
                    RoomCategory = new RoomCategory
                    {
                        Name = r.RoomCategory.Name,
                        PricePerNight = r.RoomCategory.PricePerNight
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Booking>> GetUserBookings(SieveModel sieveModel, Guid userId, CancellationToken cancellationToken)
    {
        var query = dbContext.Bookings
            .Where(b => b.UserId == userId)
            .Include(b => b.PaymentDetails)
            .AsNoTracking()
            .AsSplitQuery();
        
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    
    public async Task<List<Booking>> GetUserRecentlyVisitedHotels(Guid userId, int listCount,
        CancellationToken cancellationToken = default)
    {
        const string query = """
                                 WITH MostRecentBookings AS (
                                     SELECT
                                         b.Id,
                                         b.HotelId,
                                         b.UserId,
                                         b.CheckInDate,
                                         b.CheckOutDate,
                                         ROW_NUMBER() OVER (PARTITION BY b.HotelId ORDER BY b.CheckInDate DESC) AS rn
                                     FROM Bookings b
                                     WHERE b.UserId = @userId
                                 )
                                 SELECT TOP (@listCount)
                                     h.Name AS HotelName,
                                     h.ThumbnailUrl,
                                     h.StarRating,
                                     c.Name AS CityName,
                                     c.Country AS CountryName,
                                     c.PostalCode,
                                     p.Amount AS Price,
                                     p.PaymentMethod,
                                     b.CheckInDate,
                                     b.HotelId,
                                     b.CheckOutDate
                                 FROM MostRecentBookings b
                                 JOIN Hotels h ON b.HotelId = h.Id
                                 JOIN Cities c ON h.CityId = c.Id
                                 JOIN PaymentDetails p ON b.Id = p.BookingId
                                 WHERE b.rn = 1
                                 ORDER BY b.CheckInDate DESC;
                             """;

        var result = await dbContext.Database
            .SqlQueryRaw<RecentlyVisitedDto>(query,
                new SqlParameter("@userId", userId),
                new SqlParameter("@listCount", listCount))
            .ToListAsync(cancellationToken);
        
        var bookings = result.MapToBookings();
        return bookings;
    }
}
