using HotelBooking.Core.Data;
using HotelBooking.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Core.Services;

public class BookingService : IBookingService
{
    private readonly AppDbContext _context;

    public BookingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Booking>> GetAllAsync()
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .Include(b => b.Guest)
            .ToListAsync();
    }

    public async Task<List<Booking>> GetByUserIdAsync(string userId)
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .Include(b => b.Guest)
            .Where(b => b.Guest.UserId == userId)
            .ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(int id)
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .Include(b => b.Guest)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Booking> CreateAsync(int roomId, string userId, DateTime checkIn, DateTime checkOut)
    {
        var room = await _context.Rooms.FindAsync(roomId)
            ?? throw new InvalidOperationException("Номер не найден");

        var guest = await _context.Guests.FirstOrDefaultAsync(g => g.UserId == userId);
        if (guest == null)
        {
            throw new InvalidOperationException($"Профиль гостя не найден для пользователя {userId}");
        }

        var nights = (checkOut - checkIn).Days;
        if (nights <= 0) throw new InvalidOperationException("Некорректные даты");

        var booking = new Booking
        {
            RoomId = roomId,
            GuestId = guest.Id,
            CheckInDate = checkIn,
            CheckOutDate = checkOut,
            TotalPrice = room.PricePerNight * nights,
            Status = BookingStatus.Confirmed
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task CancelAsync(int id)
    {
        var booking = await _context.Bookings.FindAsync(id)
            ?? throw new InvalidOperationException("Бронирование не найдено");

        booking.Status = BookingStatus.Cancelled;
        await _context.SaveChangesAsync();
    }
}