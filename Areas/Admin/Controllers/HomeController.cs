using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			var listR = _context.Phongs.Include(dp => dp.IdKsNavigation).ToList();
			return View(listR);
		}

		[HttpGet]
		public async Task<IActionResult> Search(string searchTerm)
		{
			ViewData["SearchTerm"] = searchTerm;

			var results = new List<Phong>();

			if (!string.IsNullOrEmpty(searchTerm))
			{
				results = await _context.Phongs
										.Where(p => p.TenPhong.Contains(searchTerm) || p.LoaiPhong.Contains(searchTerm))
										.ToListAsync();
			}
			else
			{
				results = await _context.Phongs.ToListAsync();
			}

			return View("room",results);
		}

		public IActionResult support()
		{
			var conversations = _context.Conversations
										   .Include(c => c.Messages)
										   .Include(c => c.User)
										   .ToList();
			return View(conversations);
		}

		[HttpPost]
        public IActionResult Update(int IdPhong, bool HD)
        {
            try
            {
                // Tìm phòng cần cập nhật trong database
                var phongToUpdate = _context.Phongs.FirstOrDefault(p => p.IdPhong == IdPhong);

                if (phongToUpdate != null)
                {
                    // Cập nhật trạng thái HD của phòng
                    phongToUpdate.HD = HD;

                    // Lưu thay đổi vào database
                    _context.SaveChanges();
                }
                else
                {
                    return NotFound(); // Không tìm thấy phòng cần cập nhật
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi khi cập nhật
                ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật dữ liệu.");
            }

            // Chuyển hướng về danh sách phòng sau khi cập nhật thành công
            return RedirectToAction("room");
        }


        


	}
}
