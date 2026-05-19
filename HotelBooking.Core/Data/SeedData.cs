using HotelBooking.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Core.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        if (!context.Rooms.Any())
        {
            var rooms = new List<Room>
            {
                new() { Number = "101", Type = "Standard", PricePerNight = 100, Capacity = 2, Description = "Уютный номер с видом на двор" },
                new() { Number = "102", Type = "Standard", PricePerNight = 100, Capacity = 2, Description = "Стандартный номер" },
                new() { Number = "201", Type = "Deluxe", PricePerNight = 200, Capacity = 3, Description = "Просторный номер с балконом" },
                new() { Number = "301", Type = "Suite", PricePerNight = 350, Capacity = 4, Description = "Люкс с гостиной и спальней" }
            }
            ;

            context.Rooms.AddRange(rooms);
            context.SaveChanges();
        }

        var adminEmail = "admin@admin.hotel";
        var adminUser = userManager.FindByEmailAsync(adminEmail).Result;
        if (adminUser == null)
        {
            adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
            var createResult = userManager.CreateAsync(adminUser, "Admhotel1!").Result;

            if (!createResult.Succeeded)
            {
                throw new Exception($"Не удалось создать админа: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
            }

            var adminId = adminUser.Id;

            userManager.AddToRoleAsync(adminUser, "Admin").Wait();

            var adminGuest = new Guest
            {
                FirstName = "Админ",
                LastName = "Админов",
                Email = adminEmail,
                Phone = "",
                UserId = adminId
            };

            context.Guests.Add(adminGuest);
            context.SaveChanges();
        }
    }
}