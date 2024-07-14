using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom8.Data;
using Nhom8.Areas.Hotel.Models;
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

            int? ksID = context.KhachSans
                               .Where(q => q.UserId.Equals(userID))
                               .Select(p => p.IdKs)
                               .FirstOrDefault();

            if (ksID == null)
            {
                return NotFound();
            }

            int id = ksID.Value;

            // Lấy danh sách đặt phòng liên quan đến khách sạn
            var datPhongs = context.DatPhongs
                                   .Where(dp => dp.IdPhongNavigation.IdKs == id)
                                   .ToList();

            // Tính doanh thu theo từng ngày
            var revenuePerDay = datPhongs
                .Where(dp => dp.NgayCheckout.HasValue)
                .GroupBy(dp => new { dp.NgayCheckout.Value.Year, dp.NgayCheckout.Value.Month, dp.NgayCheckout.Value.Day })
                .Select(g => new RevenuePerDay
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Day = g.Key.Day,
                    Revenue = (float)(g.Sum(dp => (double?)dp.TongTien) ?? 0)
                })
                .OrderBy(r => r.Year)
                .ThenBy(r => r.Month)
                .ThenBy(r => r.Day)
                .ToList();

            // Trích xuất năm, tháng và ngày để truyền vào ViewBag
            List<int> years = revenuePerDay.Select(r => r.Year).Distinct().ToList();
            List<int> months = Enumerable.Range(1, 12).ToList(); // Tất cả 12 tháng

            ViewBag.Years = years;
            ViewBag.Months = months;
            ViewBag.RevenueData = revenuePerDay;

            return View(datPhongs);
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
