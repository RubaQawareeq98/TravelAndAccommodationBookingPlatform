using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Application.Features.RecentlyVisitedHotels.Dtos;
using TravelAndAccommodationBookingPlatform.Application.Persistence.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;
using TravelAndAccommodationBookingPlatform.Infrastructure.Mappers;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Bookings;

public class BookingRepository(HotelBookingManagementDbContext dbContext,
    ISieveProcessor sieveProcessor,
    IDiscountRepository discountRepository,
    IUnitOfWork unitOfWork,
    ILogger<BookingRepository> logger)
    : IBookingRepository
{
    public async Task<Result<Booking>> AddBooking(Booking booking, List<Room> rooms)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        
        await strategy.ExecuteAsync(async () =>
        {
            await unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
        
            try
            {
                foreach (var room in rooms)
                {
                    var trackedRoom = await dbContext.Rooms.FindAsync(room.Id);
                    if (trackedRoom is null)
                    {
                        throw new InvalidDataException($"Room with ID {room.Id} not found.");
                    }
                    
                    trackedRoom.UpdatedAt = DateTime.UtcNow;
                    trackedRoom.RoomCategory = room.RoomCategory;
                    booking.Rooms.Add(trackedRoom);
                }

                var totalAmount = await CalculateTotalAmount(rooms.ToList(), booking.CheckInDate, booking.CheckOutDate);
                booking.PaymentDetail.Amount = totalAmount;
                booking.PaymentDetail.PaymentNumber = 222;
                booking.PaymentDetail.PaymentDate = booking.BookingDate;
                
                dbContext.Bookings.Add(booking);
                await unitOfWork.SaveChanges();
                await unitOfWork.Commit();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                logger.LogWarning(ex, "Concurrency conflict booking rooms: {Message}", ex.Message);
                throw new DbUpdateConcurrencyException("Concurrency conflict booking rooms", ex);
            }
            catch (DbUpdateException ex) when (IsDeadlock(ex))
            {
                logger.LogWarning(ex, "Deadlock occurred during booking: {Message}", ex.Message);
                await unitOfWork.Rollback();
                throw new DbUpdateException("Deadlock occurred during booking", ex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error creating booking");
                await unitOfWork.Rollback();
                throw new InvalidOperationException("message", ex);
            }
        });
        return Result<Booking>.Success(booking);
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
            .AsSplitQuery()
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Booking?> GetBookingWithDetails(Guid id)
    {
        return await dbContext.Bookings
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
                PaymentDetail = b.PaymentDetail,
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
            }).FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Booking>> GetAllBookings(SieveModel sieveModel)
    {
        var query = dbContext.Bookings
            .Include(b => b.PaymentDetail)
            .AsNoTracking()
            .AsSplitQuery();
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
