using HotelBooking.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Core.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);  

        modelBuilder.Entity <Room> (entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Number).IsRequired().HasMaxLength(10);
            entity.Property(r => r.Type).IsRequired().HasMaxLength(50);
            entity.Property(r => r.PricePerNight).HasPrecision(18, 2);
            entity.HasIndex(r => r.Number).IsUnique();
        });

        modelBuilder.Entity <Guest> (entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(g => g.LastName).IsRequired().HasMaxLength(100);
            entity.Property(g => g.Email).IsRequired().HasMaxLength(150);
            entity.HasIndex(g => g.Email).IsUnique();
        });

        modelBuilder.Entity <Booking> (entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.TotalPrice).HasPrecision(18, 2);

            entity.HasOne(b => b.Room)
                  .WithMany(r => r.Bookings)
                  .HasForeignKey(b => b.RoomId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(b => b.Guest)
                  .WithMany(g => g.Bookings)
                  .HasForeignKey(b => b.GuestId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}