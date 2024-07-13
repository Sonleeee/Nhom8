using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nhom8.Data;
using Nhom8.ViewModels;

namespace Nhom8.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class HomeController : Controller
	{

		private readonly BookingHotelContext _context;

		public HomeController(BookingHotelContext context)
		{
			_context = context;
		}

        public IActionResult _LayoutAdmin()
		{
			var user = _context.Users.Select(dp => dp.TenKh);

			return View(user);
		}

        public IActionResult Index()
		{
			var listUser = _context.Users.Select(d => d).ToList();

			var listAD = _context.DatPhongs
			.Include(dp => dp.User) // Include user related to DatPhongs
			.Include(dp => dp.IdPhongNavigation) // Include room related to DatPhongs
			.ToList();

			var indexAD = new IndexAD()
			{
				Users = listUser,
				DatPhongs = listAD
			};
			return View(indexAD);

		}

		public IActionResult Customer()
		{
			var listCus = _context.Users.Where(p => p.Role == "CUS").ToList();
			return View(listCus);
		}

		[HttpPost]
		public string Customer(string searchString, bool notUsed)
		{
			return "From [HttpPost]Index: filter on " + searchString;
		}
		public async Task<IActionResult> SearchCus(string searchString)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            var user = from m in _context.Users
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                user = user.Where(s => s.TenKh!.Contains(searchString));
            }

            return View(await user.ToListAsync());
        }

        public IActionResult ManagerRoom()
		{
			var listMR = _context.DatPhongs
				.Include(dp => dp.User)
				.Include(dp => dp.IdPhongNavigation)
				.ThenInclude(dp => dp.IdKsNavigation)
				.ToList();

			return View(listMR);
		}

		public IActionResult room()
		{
			var listR = _context.Phongs
				.Include(dp => dp.IdKsNavigation)
				.Include(dp => dp.IdChiTietPhongNavigation)
				.ToList();
			return View(listR);
		}

		
    }
}
