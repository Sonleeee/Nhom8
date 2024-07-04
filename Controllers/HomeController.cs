using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nhom8.Models;
using Nhom8_DACS.ViewModels;
using System.Diagnostics;

namespace Nhom8_DACS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //private readonly BookingHotelContext db;

        //public HomeController(BookingHotelContext context)
        //{
        //    db = context;
        //}

        public IActionResult Index(string? destination)
        {
            //var tinh = db.KhachSans.AsQueryable();

            //if (destination != null)
            //{
            //    tinh = tinh.Where(t => t.Tinh == destination);
            //}

            //var ks_tinh = tinh.Select(t => new HotelViewModel
            //{
            //    ID_KS = t.IdKs,
            //    TenKS = t.TenKs,
            //    Tinh = t.Tinh,
            //    DanhGia = t.DanhGia,
            //    Image_KS = t.ImageKs,
            //});
            //return View(ks_tinh);
            return View();
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
