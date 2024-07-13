using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom8.Data;

namespace Nhom8_DACS.Areas.Hotel.Controllers
{
    [Area("Hotel")]
    public class HomeController : Controller
    {
        private readonly BookingHotelContext context;
        private readonly ILogger<HomeController> logger;
        public HomeController(BookingHotelContext context , ILogger<HomeController> logger)
        {
            this.context = context;
            this.logger = logger;   
        }
        int userID = 1;

        public IActionResult Index()
        {
            var hotelInfo = context.KhachSans.Where(s => s.UserId == userID).ToList();
            return View(hotelInfo);
        }

        public IActionResult CustomerRating()
        {
            return View();
        }

        public IActionResult Revenue()
        {
            return View();
        }

        public IActionResult RoomBooking()
        {
            return View();
        }

        public IActionResult RoomInfo()
        {
            return View();
        }
        
        public IActionResult Service(string searchString)
        {
            int? ksID = context.KhachSans
                  .Where(q => q.UserId.Equals(userID))
                  .Select(p => p.IdKs)
                  .FirstOrDefault();
            int id = ksID.Value;
            var DichVu = context.DichVus.Where(p => p.IdKs == id);

            if (!string.IsNullOrEmpty(searchString))
            {
                DichVu = DichVu.Where(d => d.TenDichVu.Contains(searchString));
            }
            return View(DichVu.ToList());
        }

        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddService(string tenDichVu)
        {
            
            int? ksID = context.KhachSans
                  .Where(q => q.UserId.Equals(userID))
                  .Select(p => p.IdKs)
                  .FirstOrDefault();

            DichVu dichVuMoi = new DichVu()
            {
                IdKs = ksID.Value,
                TenDichVu = tenDichVu,
                TrangThai = true,
            };
            context.DichVus.Add(dichVuMoi);
            context.SaveChanges();
            
            return RedirectToAction("Service");
        }

        public IActionResult TextTing()
        {
            return View();
        }
    }
}
