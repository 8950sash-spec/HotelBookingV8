namespace HotelBooking.Core.Models;

public class Guest
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? UserId { get; set; }

    public List<Booking> Bookings { get; set; } = new();
}