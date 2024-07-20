using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


using Nhom8.Data;
using Nhom8.Services;
using Nhom8.ViewModels;
using System.Globalization;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;
using System.Runtime.Intrinsics.Arm;

namespace Nhom8.Controllers
{
    public class Reservation1Controller : Controller
    {
        private readonly BookingHotelContext db;
        private readonly IVnPayService _vnPayservice;

        public Reservation1Controller(BookingHotelContext context, IVnPayService vnPayservice, IMapper mapper)
        {
            db = context;
            _vnPayservice = vnPayservice;
        }

        [HttpGet]
        public IActionResult Reservation1(int idP, int IdKs)
        {
            var result = (from ks in db.KhachSans
                          join p in db.Phongs on ks.IdKs equals p.IdKs into gr
                          from p in gr.DefaultIfEmpty()
                          where ks.IdKs == IdKs && p.IdPhong == idP
                          select new KhachSanPhongViewModel
                          {
                              KhachSan = ks,
                              Phong = p
                          }).FirstOrDefault();

            var checkin = HttpContext.Session.GetString("CheckinDate");            
            var checkout = HttpContext.Session.GetString("CheckoutDate");
            string dateFormat = "yyyy-MM-dd";

            DateOnly.TryParseExact(checkin, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly checkindate);
            DateOnly.TryParseExact(checkout, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly checkoutdate);


            int differenceInDays = checkoutdate.DayNumber - checkindate.DayNumber;
            if (differenceInDays == 0)
            {
                differenceInDays = 1;
            }
            var userMail = "";
            var userName = "";
            if (User.Identity.IsAuthenticated)
            {
                userMail = User.FindFirstValue(ClaimTypes.Email);
                userName = User.FindFirstValue(ClaimTypes.Name);
            }

            double? roomPriceNullable = result.Phong?.GiaPhong; // Đảm bảo result.Phong không phải là null
            double roomPrice = roomPriceNullable.GetValueOrDefault(0); // Lấy giá trị hoặc 0 nếu null

            var viewModel = new ReservationVM
            {
                Name = userName,
                Email = userMail,
                Sdt = "",
                KhachSanPhong = result,
                CheckinDate = checkin,
                CheckoutDate = checkout,
                Price = (int)roomPrice * differenceInDays // Tính giá tiền
            };


            return View(viewModel);
        }   

        [HttpPost]
        public IActionResult Reservation1(ReservationVM model, string? payment)
        {
            if (payment == "Thanh toán VnPay")
            {
                var vnPayModel = new VnPaymentRequestModel
                {
                    //Amount = Cart.Sum(p => p.ThanhTien),
                    //CreatedDate = DateTime.Now,
                    //Description = $"{model.HoTen} {model.DienThoai}",
                    //FullName = model.HoTen,
                    //OrderId = new Random().Next(1000, 100000)

                    TongTien =800000,
                    CreatedDate = DateTime.Now,
                    //Mota = $"Lê Hoàng Sơn {model.Sdt}",
                    TenKH = model.Name,
                    OrderId = new Random().Next(1000, 100000),

                };
                return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
            }
            //if (payment == "Thanh toán tại khách sạn")
            //{
            //    var dp = db.DatPhongs.Where
            //}

            //return View(model);
            return View();
        }

        public IActionResult PaymentSuccess()
        {
            return View("PaymentSuccess");
        }

        public IActionResult PaymentFaile()
        {
            return View();
        }

        public IActionResult PaymentCallBack()
        {
            var response = _vnPayservice.PaymentExcute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VnPay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFaile");
            }

            TempData["Message"] = $"Thanh toán thành công";

            return RedirectToAction("PaymentSuccess");
        }
        public IActionResult Reservation1()
        {
            return View();
        }
    }
}
