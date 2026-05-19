using HotelBooking.Core.Models;

namespace HotelBooking.Core.Services;

public interface IRoomService
{
    Task<List<Room>> GetAllAsync();
    Task<Room?> GetByIdAsync(int id);
    Task<List<Room>> GetAvailableAsync(DateTime checkIn, DateTime checkOut);
    Task<Room> AddAsync(Room room);
    Task DeleteAsync(int id);
}