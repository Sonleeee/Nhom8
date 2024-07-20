using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using Nhom8.Services.Email;
using Nhom8.ViewModels;
using Nhom8.Helpers;
using Microsoft.AspNetCore.Http;
using Nhom8.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Nhom8.Controllers
{
    public class AuthenController : Controller
    {
        private readonly BookingHotelContext db;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public AuthenController(BookingHotelContext context, IMapper mapper, IEmailSender emailSender)
        {
            db = context;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        #region đăng nhập
        [HttpGet]
        public IActionResult Login(string? url)
        {
            ViewBag.ReturnUrl = url;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var khachhang = db.Users.SingleOrDefault(kh => kh.Tk == model.Email);
                if (khachhang == null)
                {
                    ModelState.AddModelError("Loi", "Sai thông tin đăng nhập!");
                    Console.WriteLine("sai thong tin dang nhapj");
                }
                else
                {
                    if (khachhang.Mk != model.Password.ToMd5Hash(khachhang.RandomKey))
                    {
                        ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                        Console.WriteLine("sai thong ");
                    }
                    else
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, khachhang.Email),
                            new Claim(ClaimTypes.Name, khachhang.TenKh),
                            new Claim(ClaimTypes.Role, khachhang.Role),                            
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(new ClaimsPrincipal(claimsPrincipal));

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            if (khachhang.Role=="KS")
                            {
                                return RedirectToAction("home", "hotel");
                            }
                            if (khachhang.Role == "AD")
                            {
                                return RedirectToAction("home", "admin");
                            }
                            return Redirect("/");
                        }
                    }
                }
            }
            return View();
        }

        public IActionResult Profile()
        {
            var userMail = "";
            var userName = "";
            if (User.Identity.IsAuthenticated)
            {
                userMail = User.FindFirstValue(ClaimTypes.Email);
                userName = User.FindFirstValue(ClaimTypes.Name);
            }
            var user = db.Users.Where(u => u.TenKh == userName && u.Email == userMail).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("login", "authen");
            }

            var model = new ProfileViewModel
            {
                User = user,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Profile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userMail = "";
                var userName = "";
                if (User.Identity.IsAuthenticated)
                {                    
                    userName = User.FindFirstValue(ClaimTypes.Name);
                    var user = db.Users.Where(u => u.TenKh == userName).FirstOrDefault();
                    
                    user.Email = model.User.Email;
                    user.TenKh = model.User.TenKh;
                    user.Sdt = model.User.Sdt;
                    user.Mk = model.User.Mk;
                    db.Users.Update(user);
                    db.SaveChanges();
                }
                else
                {
                    return RedirectToAction("login", "authen");
                }
            }
            return View(model);
        }

        #endregion

        #region Lognin-GG

        public async Task GGLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var userClaims = result.Principal.Identities.FirstOrDefault()?.Claims;
            if (userClaims == null)
            {
                return RedirectToAction("Index", "Home"); // Điều hướng về trang chủ nếu không có thông tin người dùng
            }
            var email = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var existingUser = db.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser == null)
            {
                // Nếu không có người dùng, tạo mới người dùng
                var newUser = new User
                {
                    Email = email,
                    TenKh = name,
                    Role = "CUS"
                    // Thêm các thuộc tính khác nếu cần
                };

                db.Users.Add(newUser);
                await db.SaveChangesAsync();
            }
            else
            {
                // Cập nhật thông tin người dùng nếu cần
                existingUser.TenKh = name;
                // Cập nhật các thuộc tính khác nếu cần
                await db.SaveChangesAsync();
            }

            // Điều hướng về trang chủ
            return RedirectToAction("Index", "Home");
        }
    
        #endregion

        #region đăng ký

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile hinh)
        {

            if (model.ConfirmPassword == model.Mk)
            {
                var khachhang = _mapper.Map<User>(model);
                khachhang.RandomKey = MyUtil.GenerateRandomKey();
                khachhang.Mk = model.Mk.ToMd5Hash(khachhang.RandomKey);
                khachhang.Email = model.Email;
                khachhang.Role = "CUS";
                khachhang.Tk = model.Email;
                khachhang.Role = "CUS";
                string OTP = MyUtil.RandomOTP().ToString();
                HttpContext.Session.SetString("OTP", OTP);
                var subject = OTP.ToString() + " là mã OTP của bạn";
                var content = "Vui lòng không chia sẽ mã này với bất kì ai!!";
                try
                {
                    await _emailSender.SendEmailAsync(model.Email, subject, content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                if (hinh != null)
                {
                    khachhang.Img = MyUtil.UploadHinh(hinh, "profile");
                }
                db.Add(khachhang);
                db.SaveChanges();
                return RedirectToAction("login", "authen");
            }

            return View();
        }

        public IActionResult ConFirm()
        {

            return View();
        }

        #endregion

        public IActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Forgot(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(u => u.Email == model.Email).FirstOrDefault();
                if (user != null)
                {
                    // Tạo token reset mật khẩu và gửi email
                    var token = Guid.NewGuid().ToString();
                    // Lưu token vào database hoặc gửi email với token này
                    try
                    {
                        var callbackUrl = Url.Action("ResetPassword", "Authen", new { token = token, email = user.Email }, protocol: HttpContext.Request.Scheme);
                        await _emailSender.SendEmailAsync(model.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                // Thông báo cho người dùng rằng email đã được gửi (không tiết lộ rằng email không tồn tại)
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return BadRequest("Token hoặc email không hợp lệ.");
            }
            // Tạo một model với thông tin token và email
            var model = new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Xác minh token và cập nhật mật khẩu
                var user = db.Users.FirstOrDefault(u => u.Email == model.Email);

                if (user != null)
                {
                    // Giả sử bạn lưu token trong cơ sở dữ liệu và so sánh ở đây
                    // Cập nhật mật khẩu mới
                    user.Mk = model.NewPassword;
                    db.Update(user);
                    await db.SaveChangesAsync();

                    return RedirectToAction("ResetPasswordConfirmation");
                }

                // Nếu người dùng không tồn tại
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra.");
            }

            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    

    [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Đăng xuất khỏi ứng dụng của bạn
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

    }
}