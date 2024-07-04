using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Nhom8_DACS.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class HomeController : Controller
	{

		//private readonly BookingHotelContext _context;

		//public HomeController(BookingHotelContext context)
		//{
		//	_context = context;
		//}


		public IActionResult Index()
		{
			//var listAD = _context.DatPhongs
			//.Include(dp => dp.User) // Include user related to DatPhongs
			//.Include(dp => dp.IdPhongNavigation) // Include room related to DatPhongs
			//.ToList();

			//return View(listAD);

			return View();
		}

		public IActionResult Customer()
		{
			//var listCus = _context.Users.Where(p => p.Role == "CUS").ToList();
			//return View(listCus);
			return View();
		}

		public IActionResult ManagerRoom()
		{
			//var listMR = _context.DatPhongs
			//	.Include(dp => dp.User)
			//	.Include(dp => dp.IdPhongNavigation)
			//	.ToList();
			//return View(listMR);
			return View();
		}

		public IActionResult room()
		{
			//var listR = _context.Phongs.Select(p => p).ToList();
			//return View(listR);
			return View();
		}

		public IActionResult support()
		{
			return View();
		}


	}
}
