using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nhom8.Helpers;
using Nhom8.Services.Email;
using Nhom8.Data;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ vào container
builder.Services.AddControllersWithViews();

// Đăng ký DbContext với Dependency Injection
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

// Cấu hình pipeline yêu cầu HTTP
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
