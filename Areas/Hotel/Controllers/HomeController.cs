using Microsoft.AspNetCore.Mvc;


namespace Nhom8_DACS.Areas.Hotel.Controllers
{
    [Area("Hotel")]
    public class HomeController : Controller
    {
        //private readonly BookingHotelContext _context;

        //public HomeController(BookingHotelContext context)
        //{
        //    _context = context;
        //}

        public IActionResult Index()
        {
  
            return View();
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

        public IActionResult Service()
        {
            return View();
        }

        public IActionResult TextTing()
        {
            return View();
        }

    }
}
