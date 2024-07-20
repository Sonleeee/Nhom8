using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nhom8.Data;
using Nhom8.Helpers;
using Nhom8.Services;
using Nhom8.Services.Email;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ vào container
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IVnPayService, VnPayService>();

builder.Services.AddDbContext<BookingHotelContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("Booking_Hotel"));
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(op =>
{
    op.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout
    op.Cookie.HttpOnly = true; // Cookie chỉ sử dụng cho HTTP
    op.Cookie.IsEssential = true; // Cookie này cần thiết để sử dụng session
});

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    op.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    op.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    op.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(op =>
    {
        //op.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
        op.LoginPath = "/Authen/Login";
        op.LogoutPath = "/Authen/Logout";
        op.AccessDeniedPath = "/AccessDenied";
    })
    .AddGoogle(op =>
    {
        op.ClientId = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_CLIENT_ID");
        op.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_CLIENT_SECRET");
        ////op.CallbackPath = "/signin-google";
    })
    ;

var app = builder.Build();

// Cấu hình pipeline yêu cầu HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();

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
