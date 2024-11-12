using ABCRetailers.Data;  
using ABCRetailers.Models;  
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection1")));
// Add services to the container.
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
  //  options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services with custom User model and Role management.
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;  // Customize this as needed
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();  // For email confirmation, password resets, etc.

// Add Razor Pages and MVC services
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");  // Friendly error page in production
    app.UseHsts();  // Enforce HTTPS in production environments
}

app.UseHttpsRedirection();   // Redirect HTTP requests to HTTPS
app.UseStaticFiles();        // Serve static files (CSS, JS, images, etc.)

app.UseRouting();  // Enable routing for controllers and Razor pages

app.UseAuthentication();  // Enable authentication middleware
app.UseAuthorization();   // Enable authorization middleware

// Map Razor Pages (for Identity: Register, Login, etc.)
app.MapRazorPages();

// Set up default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
