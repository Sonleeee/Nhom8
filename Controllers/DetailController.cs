using Microsoft.AspNetCore.Mvc;
using Nhom8.Data;
using Nhom8.ViewModels;

namespace Nhom8.Controllers
{
    public class DetailController : Controller
    {
        private readonly BookingHotelContext db;

        public DetailController(BookingHotelContext context)
        {
            db = context;
        }
        public IActionResult Detail()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Detail(int idKs)
        {
            var khachSan = db.KhachSans.SingleOrDefault(ks => ks.IdKs == idKs);
            //if (khachSan == null)
            //{
            //    return HttpNotFound();
            //}

            // Lấy danh sách phòng của khách sạn và nhóm theo loại phòng
            var groupedPhongs = db.Phongs.Where(p => p.IdKs == idKs)
                                         .GroupBy(p => p.LoaiPhong)
                                         .ToList();

            var phongIds = groupedPhongs.SelectMany(g => g.Select(p => p.IdPhong)).ToList();
            var chiTietPhongs = db.ChiTietPhongs.Where(ctp => phongIds.Contains(ctp.IdChiTietPhong)).ToList();
            var dichVus = db.DichVus.Where(dv => dv.IdKs == idKs).ToList();

            var viewModel = new DetailViewModel
            {
                _KhachSan = khachSan,
                _GroupedPhongs = groupedPhongs.Select(g => new GroupedPhongViewModel
                {
                    LoaiPhong = g.Key,
                    Phongs = g.ToList()
                }).ToList(),
                _ChiTietPhongs = chiTietPhongs, // Assuming you want all chi tiết phòng
                _DichVu = dichVus
            };

            return View(viewModel);
        }
    }
}
