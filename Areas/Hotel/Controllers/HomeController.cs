using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom8.Data;

namespace Nhom8_DACS.Areas.Hotel.Controllers
{
    [Area("Hotel")]
    public class HomeController : Controller
    {
        private readonly BookingHotelContext context;
        
        public HomeController(BookingHotelContext context)
        {
            this.context = context;
            
        }
        int userID = 1;

        public IActionResult Index()
        {
            ViewBag.ActivePage = "Index";
            var hotelInfo = context.KhachSans.Where(s => s.UserId == userID).ToList();
            return View(hotelInfo);
        }

        public IActionResult CustomerRating()
        {
            ViewBag.ActivePage = "CustomerRating";
            return View();
        }

        public IActionResult Revenue()
        {
            ViewBag.ActivePage = "Revenue";
            return View();
        }

        public IActionResult RoomBooking()
        {
            ViewBag.ActivePage = "RoomBooking";
            return View();
        }

        public IActionResult RoomInfo()
        {
            ViewBag.ActivePage = "RoomInfo";
            int? ksID = context.KhachSans
                 .Where(q => q.UserId.Equals(userID))
                 .Select(p => p.IdKs)
                 .FirstOrDefault();
            int id = ksID.Value;
            var Phong = context.Phongs.Where(p => p.IdKs == id);
            return View(Phong.ToList());
        }
        
        public IActionResult Service(string searchString)
        {
            ViewBag.ActivePage = "Service";
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
            ViewBag.ActivePage = "TextTing";
            return View();
        }
    }
}
