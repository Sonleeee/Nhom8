using Microsoft.AspNetCore.Mvc;
using Nhom8.Data;

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
    }
}
