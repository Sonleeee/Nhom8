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

<<<<<<< HEAD
		public IActionResult _LayoutAdmin()
		{
			var user = _context.Users.Select(dp => dp.TenKh);

			return View(user);
		}
=======
        
>>>>>>> dat

		public IActionResult Index()
		{
			var listUser = _context.Users.Select(d => d).ToList();

			var listAD = _context.DatPhongs.Select(d => d).ToList();

            var listRoom = _context.Phongs.Select(d => d).ToList();

			var indexAD = new IndexAD()
			{
				Users = listUser,
				DatPhongs = listAD,
                Phongs =  listRoom
			};
			return View(indexAD);

		}

		/*public IActionResult Customer()
		{
			var listCus = _context.Users.Where(p => p.Role == "CUS").ToList();
			return View(listCus);
		}*/

		public IActionResult Customer(string searchString)
		{
			var customers = _context.Users.Where(p => p.Role == "CUS");

			if (!string.IsNullOrEmpty(searchString))
			{
				customers = customers.Where(s => s.TenKh.Contains(searchString));
			}

<<<<<<< HEAD
			return View("room", results);
		}

		public IActionResult support()
		{
			var conversations = _context.Conversations
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
					phongToUpdate.Hd = HD;

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



=======
			return View(customers.ToList());
		}

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(); // Xử lý khi không tìm thấy người dùng
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Customer));
        }


        public IActionResult ManagerRoom(string searchString)
		{
			var rooms = _context.DatPhongs
				.Include(dp => dp.User)
				.Include(dp => dp.IdPhongNavigation)
				.ThenInclude(dp => dp.IdKsNavigation)
				.AsQueryable();

			if (!string.IsNullOrEmpty(searchString))
			{
				rooms = rooms.Where(r =>
					(r.IdPhongNavigation != null && r.IdPhongNavigation.TenPhong != null && r.IdPhongNavigation.TenPhong.Contains(searchString)) ||
					(r.User != null && r.User.TenKh != null && r.User.TenKh.Contains(searchString)) ||
					(r.IdPhongNavigation != null && r.IdPhongNavigation.IdKsNavigation != null && r.IdPhongNavigation.IdKsNavigation.TenKs.Contains(searchString))
				);
			}

			return View(rooms.ToList());
		}
>>>>>>> dat



        public IActionResult room(string searchString)
        {
            var rooms = _context.Phongs
                .Include(p => p.IdKsNavigation)
                .Include(p => p.IdChiTietPhongNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                rooms = rooms.Where(r =>
                    r.TenPhong.Contains(searchString) ||
                    r.IdKsNavigation.TenKs.Contains(searchString) ||
                    r.IdChiTietPhongNavigation.SlGiuong == int.Parse(searchString)
                     // Ví dụ cho trường hợp tìm kiếm theo IdPhong
                );
            }

            return View(rooms.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Updates(int id, bool hd)
        {
            var room = await _context.Phongs.FindAsync(id);

            if (room == null)
            {
                return NotFound(); // Xử lý khi không tìm thấy phòng
            }

			if(room.Hd == true)
                room.Hd = hd;

            else
                room.Hd = !hd;



            // Cập nhật trạng thái của phòng

            _context.Update(room);
            await _context.SaveChangesAsync();

            return RedirectToAction("room"); // Chuyển hướng về action hiển thị danh sách phòng, hãy đảm bảo rằng "Index" là tên chính xác của action
        }




        public IActionResult support()
        {
            return View();
        }


    }
}
