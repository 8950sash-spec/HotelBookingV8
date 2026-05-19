using HotelBooking.Core.Data;
using HotelBooking.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Core.Services;

public class RoomService : IRoomService
{
    private readonly AppDbContext _context;

    public RoomService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Room>> GetAllAsync()
    {
        return await _context.Rooms.ToListAsync();
    }

    public async Task<Room?> GetByIdAsync(int id)
    {
        return await _context.Rooms.FindAsync(id);
    }

    public async Task<List<Room>> GetAvailableAsync(DateTime checkIn, DateTime checkOut)
    {
        var bookedRoomIds = await _context.Bookings
            .Where(b => b.Status == BookingStatus.Confirmed &&
                        b.CheckInDate < checkOut &&
                        b.CheckOutDate > checkIn)
            .Select(b => b.RoomId)
            .Distinct()
            .ToListAsync();

        return await _context.Rooms
            .Where(r => !bookedRoomIds.Contains(r.Id))
            .ToListAsync();
    }

    public async Task<Room> AddAsync(Room room)
    {
        room.IsAvailable = true;
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task DeleteAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
            throw new InvalidOperationException("Номер не найден");

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }
}