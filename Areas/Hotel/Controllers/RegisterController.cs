using Microsoft.AspNetCore.Mvc;
using Nhom8.Data;

namespace Nhom8_DACS.Areas.Hotel.Controllers
{
    [Area("Hotel")]
    public class RegisterController : Controller
    {
        private readonly BookingHotelContext context;
        private readonly IWebHostEnvironment environment;

        public RegisterController(BookingHotelContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        int userID = 1;
        public IActionResult Register()
        {
            var Tinhs = context.Tinhs.ToList();
            Console.WriteLine($"Số lượng tỉnh: {Tinhs.Count}"); // In số lượng tỉnh ra console hoặc log để kiểm tra
            ViewBag.Tinhs = Tinhs;
            return View();
        }
        [HttpPost]
        public IActionResult Register(string tenKs, string diaChi, int tinhID)
        {
            

            return View();
        }
    }
}
