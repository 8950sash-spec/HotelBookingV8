namespace HotelBooking.Core.Models;

public class Booking
{
    public int Id { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public int GuestId { get; set; }
    public Guest Guest { get; set; } = null!;
}

public enum BookingStatus
{
    Confirmed,
    Cancelled,
    Completed
}