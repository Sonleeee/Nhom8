using Microsoft.AspNetCore.Mvc;
using static NuGet.Packaging.PackagingConstants;

using Microsoft.EntityFrameworkCore;
using Nhom8_DACS.ViewModels;
using System;
using NuGet.Protocol.Plugins;


namespace Nhom8_DACS.Controllers
{
    public class ResearchController : Controller
    {
        //private BookingHotelContext db;

        //public ResearchController(BookingHotelContext context)
        //{
        //    db = context;
        //}

        public IActionResult research()
        {
            return View();
        }

        //location=Hồ+chí+minh&ngay_nhan_phong=2024-06-28&ngay_tra_phong=2024-06-30&nguoi_lon=4&tre_em=1&phong=2
        [HttpGet]
        public IActionResult searHotel(string? location, string? ngay_nhan_phong, string? ngay_tra_phong, int? nguoi_lon, int? tre_em, int? phong)
        {
            string loaiphong = "";

            if (phong == 1)
            {
                
            }

            if (nguoi_lon == 1)
            {
                loaiphong = "Đơn";
            }
            else if (nguoi_lon == 2)
            {
                loaiphong = "Đôi";
            }
            else
            {
                loaiphong = "Gia đình";
            }
            

            //var query = db.KhachSans
            //            .Join(db.Phongs,
            //            ks => ks.IdKs,
            //            p => p.IdKs,
            //            (ks, p) => new { ks, p })
            //            .Where(ks => ks.ks.Tinh.Equals(location) && ks.ks.)
            //            .Select(rs => new HotelViewModel
            //            {
            //                  ID_KS = ,
            //                  TenKS = rs.TenKs,
            //                  DanhGia = rs.DanhGia,
            //                  DiaChi = rs.DiaChi,
            //                  TenPhong = rs.TenPhong,
            //                  GiaPhong = rs.GiaPhong
            //            });
            
            

            return View(loaiphong);
        }
    }
}
