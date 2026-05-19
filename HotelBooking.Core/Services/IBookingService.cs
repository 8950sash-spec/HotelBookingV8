using HotelBooking.Core.Models;

namespace HotelBooking.Core.Services;

public interface IBookingService
{
    Task<List<Booking>> GetAllAsync();
    Task<List<Booking>> GetByUserIdAsync(string userId);
    Task<Booking?> GetByIdAsync(int id);
    Task<Booking> CreateAsync(int roomId, string userId, DateTime checkIn, DateTime checkOut);
    Task CancelAsync(int id);
}