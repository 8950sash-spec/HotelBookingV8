using HotelBooking.Core.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager <IdentityUser> _userManager;
    private readonly SignInManager <IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
        if (result.Succeeded)
        {
            return Redirect("/");
        }
        ModelState.AddModelError("", "Неверный email или пароль");
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password, string firstName, string lastName)
    {
        var user = new IdentityUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();

            var existingGuest = await context.Guests.FirstOrDefaultAsync(g => g.Email == email);
            if (existingGuest == null)
            {
                var guest = new HotelBooking.Core.Models.Guest
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Phone = "",
                    UserId = user.Id
                };
                context.Guests.Add(guest);
                await context.SaveChangesAsync();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Redirect("/");
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/");
    }
}