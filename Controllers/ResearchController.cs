using Microsoft.AspNetCore.Mvc;
using static NuGet.Packaging.PackagingConstants;

using Microsoft.EntityFrameworkCore;
using Nhom8.ViewModels;
using System;
using NuGet.Protocol.Plugins;
using Nhom8.Data;


namespace Nhom8_DACS.Controllers
{
    public class ResearchController : Controller
    {
        private BookingHotelContext db;

        public ResearchController(BookingHotelContext context)
        {
            db = context;
        }

        public IActionResult research()
        {

            return View();
        }

        //location=Hồ+chí+minh&ngay_nhan_phong=2024-06-28&ngay_tra_phong=2024-06-30&nguoi_lon=4&tre_em=1&phong=2
        [HttpGet]
        public IActionResult research(int? idTinh, DateOnly? ngay_nhan_phong, DateOnly? ngay_tra_phong, int? nguoi_lon, int? tre_em, int? phong, int? IdKs)
        {
            var result = from ks in db.KhachSans
                         join t in db.Tinhs on ks.TinhId equals t.IdTinh
                         join p in db.Phongs on ks.IdKs equals p.IdKs
                         join ctp in db.ChiTietPhongs on p.IdPhong equals ctp.IdPhong
                         join dp in db.DatPhongs on p.IdPhong equals dp.IdPhong into dpGroup
                         from dp in dpGroup.DefaultIfEmpty()
                         where p.Hd == true
                         select new { ks, p, dp, t, ctp };

            if (idTinh.HasValue)
            {
                result = result.Where(r => r.t.IdTinh == idTinh);
            }

            if (IdKs.HasValue)
            {
                result = result.Where(r => r.ks.IdKs == IdKs);
            }

            if (ngay_nhan_phong.HasValue && ngay_tra_phong.HasValue)
            {
                result = result.Where(r => !(r.dp != null && r.dp.NgayCheckin <= ngay_tra_phong && r.dp.NgayCheckout >= ngay_nhan_phong));
            }


            int sl_nguoi = (nguoi_lon ?? 0) + (tre_em ?? 0);
            double sl_giuong = Math.Ceiling(sl_nguoi / 2.0);

            result = result.Where(r => r.ctp.SlGiuong >= sl_giuong);

            // Tạo danh sách kết quả phù hợp
            var groupedResult = from r in result
                                group r by r.ks into g
                                select new ResearchVM
                                {
                                    KhachSan = g.Key,
                                    Phong = g.OrderBy(gp => gp.p.GiaPhong).FirstOrDefault().p,
                                    ChiTietPhong = g.FirstOrDefault().ctp
                                };

            // Tạo danh sách cuối cùng
            var finalResult = new List<ResearchVM>();
            foreach (var item in groupedResult)
            {
                if (item.ChiTietPhong != null && item.ChiTietPhong.SlGiuong > 0)
                {
                    int availableRooms = (int)Math.Ceiling((double)sl_nguoi / (double)(item.ChiTietPhong.SlGiuong * 2)); // Giả định mỗi giường chứa 2 người
                    if (!phong.HasValue || (phong.HasValue && availableRooms <= phong.Value))
                    {
                        finalResult.Add(item);
                    }
                }
            }

            return View(finalResult);

        }
    }
}
