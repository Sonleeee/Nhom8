using Microsoft.AspNetCore.Mvc;
using Nhom8.Data;
using Nhom8.Models;
using Nhom8.ViewModels;
using System.Diagnostics;
using static Nhom8.ViewModels.KhachSanTinhViewModel;

namespace Nhom8.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly BookingHotelContext db;

        public HomeController(BookingHotelContext context)
        {
            db = context;
        }

        public IActionResult Index(string? destination)
        {
            var ks = db.KhachSans.Select(k => new Nhom8.ViewModels.KhachSanS
            {
                Id = k.IdKs,
                Name = k.TenKs ?? "no name",
                Img = k.ImageKs ?? "",
                Star = k.Star.GetValueOrDefault(),
                Tinh = k.Tinh.Tinh1 ?? "",

            }).AsQueryable();

            var tinh = db.Tinhs.Select(t => new TinhS
            {
                Id = t.IdTinh,
                Name = t.Tinh1 ?? "",
                Img = t.ImgTinh ?? "",   
                
            }).ToList();

            var viewModel = new KhachSanTinhViewModel
            {
                KhachSans = ks,
                Tinhs = tinh,
            };

            return View(viewModel);
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
