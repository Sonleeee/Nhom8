using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Nhom8.Data;
using Nhom8.Helpers;
using Nhom8.Services.Email;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ??ng ký DBContext v?i dependency Injection
builder.Services.AddDbContext<BookingHotelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Booking_Hotel")));

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    op.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie(op =>
    {
        op.LoginPath = "/Authen/Login";
        op.AccessDeniedPath = "/AccessDenied";
    })
    .AddGoogle(op =>
    {
        op.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientID").Value;
        op.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
