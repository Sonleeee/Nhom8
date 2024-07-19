using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nhom8.Data;
using Nhom8.Services;
using Nhom8.ViewModels;

namespace Nhom8.Controllers
{
    public class Reservation1Controller : Controller
    {
        private readonly BookingHotelContext db;
        private readonly IVnPayService _vnPayservice;

        public Reservation1Controller(BookingHotelContext context, IVnPayService vnPayservice)
        {
            db = context;
            _vnPayservice = vnPayservice;
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

                    TongTien = 1000000,
                    CreatedDate = DateTime.Now,
                    Mota = $"Lê Hoàng Sơn {model.DienThoai}",
                    TenKH = "Lê Hoàng Sơn",
                    OrderId = new Random().Next(1000, 100000),

                };
                return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
            }

            return View(model);
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
