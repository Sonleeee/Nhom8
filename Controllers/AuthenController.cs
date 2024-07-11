using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Nhom8.Data;
using Nhom8.Services.Email;
using Nhom8.ViewModels;

namespace Nhom8.Controllers
{
    //[Route("Authen")]
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
                var khachhang = db.Users.SingleOrDefault(kh => kh.Tk == model.Email && kh.Mk == model.Password);
                if (khachhang == null)
                {
                    ModelState.AddModelError("Loi", "Sai thông tin đăng nhập!");
                }
                else
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, model.Email),
                        new Claim(ClaimTypes.Role, "KH"),
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
                        return Redirect("/");
                    }
                }
            }
            return View();
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
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

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            return Json(claims);
        }
        #endregion




        #region đăng ký

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model, IFormFile hinh)
        {

            //if (model.ConfirmPassword == model.Mk)
            //{
            //    var khachhang = _mapper.Map<User>(model);
            //    khachhang.RandomKey = MyUtil.GenerateRandomKey();
            //    khachhang.Mk = model.Mk.ToMd5Hash(khachhang.RandomKey);
            //    khachhang.Role = "";
            //    khachhang.Tk = model.Email;
            //    khachhang.Otp = MyUtil.RandomOTP();

            //    if (hinh != null)
            //    {
            //        khachhang.Img = MyUtil.UploadHinh(hinh, "profile");
            //    }
            //    db.Add(khachhang);
            //    db.SaveChanges();
            //    return RedirectToAction("login", "authen");
            //}

            return View();
        }

        #endregion

        public IActionResult Forgot()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }


        


    }
}