using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nhom8.Data;
using Nhom8.Helpers;
using Nhom8.Services.Email;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ vào container
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BookingHotelContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("Booking_Hotel"));
});

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    op.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    op.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(IdentityConstants.TwoFactorUserIdScheme, op =>
    {
        op.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
        op.LoginPath = "/Authen/Login";
        op.AccessDeniedPath = "/AccessDenied";
    })
    .AddGoogle(op =>
    {
        op.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientID").Value;
        op.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
        op.CallbackPath = "/signin-google";
    })
    ;

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
