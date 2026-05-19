using HotelBooking.Core.Data;
using HotelBooking.Core.Services;
using HotelBooking.Web.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext SQLite
builder.Services.AddDbContext <AppDbContext > (options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity <IdentityUser, IdentityRole > (options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores <AppDbContext > ()
    .AddDefaultTokenProviders();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped <IRoomService, RoomService> ();
builder.Services.AddScoped <IBookingService, BookingService> ();

var app = builder.Build();

// Миграции + сид + админ
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService <AppDbContext > ();
    var userManager = scope.ServiceProvider.GetRequiredService <UserManager <IdentityUser >> ();
    var roleManager = scope.ServiceProvider.GetRequiredService <RoleManager <IdentityRole >> ();

    context.Database.Migrate();

    if (!roleManager.RoleExistsAsync("Admin").Result)
    {
        roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
    }

    SeedData.Initialize(context, userManager);
}

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorComponents <App> ()
    .AddInteractiveServerRenderMode();

app.Run();