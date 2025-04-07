using TirtaOptima.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);

// Add Razor View support
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//Setup Database
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql("server=localhost;port=3306;database=tirtaoptima;user=root;password=",
   Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql")));

// Setup Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Index";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    });

// Authorization by role (claim-based)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Administrator Sistem"));
    options.AddPolicy("Penagih", policy => policy.RequireClaim("Petugas Penagihan"));
    options.AddPolicy("Pengelola", policy => policy.RequireClaim("Petugas Pengelola_Piutang"));
    options.AddPolicy("Pimpinan", policy => policy.RequireClaim("Pimpinan"));
});

// Password hashing config
builder.Services.Configure<PasswordHasherOptions>(option =>
{
    option.IterationCount = 12000;
});

// Session setup
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Penagihan.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Session harus sebelum auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();