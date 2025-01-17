﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nhom8.Areas.Admin.Models;
using Nhom8.Data;
using Nhom8.ViewModels;
using System.Globalization;

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



		public IActionResult Index()
		{
			var listUser = _context.Users.Select(d => d).ToList();

			var listAD = _context.DatPhongs.Select(d => d).ToList();

			var listRoom = _context.Phongs.Select(d => d).ToList();

			var indexAD = new IndexAD()
			{
				Users = listUser,
				DatPhongs = listAD,
				Phongs = listRoom
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

            var datPhongs = _context.DatPhongs.Where(dp => dp.UserId == id);
            _context.DatPhongs.RemoveRange(datPhongs);

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



		public IActionResult room(string searchString)
		{
            var rooms = _context.Phongs
			.Include(p => p.IdKsNavigation)
			.ToList();

            var CTP = _context.ChiTietPhongs.ToList();

            var roomAD = new RoomAD()
            {
                Phongs = rooms,
                CTP = CTP
            };

            if (!string.IsNullOrEmpty(searchString))
            {
                int bedCount;
                bool isNumber = int.TryParse(searchString, out bedCount);

                roomAD.Phongs = roomAD.Phongs.Where(r =>
                    r.TenPhong.Contains(searchString) ||
                    r.IdKsNavigation.TenKs.Contains(searchString)).ToList();
            }

            return View(roomAD);
        }

        
		public async Task<IActionResult> DeleteR(int id)
		{
            var phong = await _context.Phongs
			.Include(p => p.ChiTietPhongs)
           .ThenInclude(ctp => ctp.TienNghis)
			.FirstOrDefaultAsync(p => p.IdPhong == id);

            if (phong == null)
            {
                return NotFound(); // Xử lý khi không tìm thấy phòng
            }

            // Xóa các bản ghi liên quan trong bảng TienNghis
            foreach (var chiTietPhong in phong.ChiTietPhongs)
            {
                _context.TienNghis.RemoveRange(chiTietPhong.TienNghis);
            }

            // Lưu thay đổi để xóa các TienNghis trước
            await _context.SaveChangesAsync();

            // Xóa các bản ghi liên quan trong bảng ChiTietPhongs
            _context.ChiTietPhongs.RemoveRange(phong.ChiTietPhongs);

            // Lưu thay đổi để xóa các ChiTietPhongs
            await _context.SaveChangesAsync();

            // Xóa phòng
            _context.Phongs.Remove(phong);

            // Lưu thay đổi cuối cùng
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(room));
    }

        [HttpPost]
		public async Task<IActionResult> Updates(int id, bool hd)
		{
			var room = await _context.Phongs.FindAsync(id);

			if (room == null)
			{
				return NotFound(); // Xử lý khi không tìm thấy phòng
			}

			if (room.Hd == true)
				room.Hd = hd;

			else
				room.Hd = !hd;



			// Cập nhật trạng thái của phòng

			_context.Update(room);
			await _context.SaveChangesAsync();

			return RedirectToAction("room"); // Chuyển hướng về action hiển thị danh sách phòng, hãy đảm bảo rằng "Index" là tên chính xác của action
		}

        public IActionResult Revenue()
        {
            var Revenue = _context.DatPhongs
                .Include(dp => dp.IdPhongNavigation)
                .ThenInclude(dp => dp.IdKsNavigation)
                .ToList();

            var revenuePerDay = _context.DatPhongs
               .Where(dp => dp.NgayCheckout.HasValue)
               .GroupBy(dp => new { dp.NgayCheckout.Value.Year, dp.NgayCheckout.Value.Month })
               .Select(g => new RevenuePerDay
               {
                   Year = g.Key.Year,
                   Month = g.Key.Month,
                   Revenue = (float)((g.Sum(dp => (double?)dp.TongTien) * 5 / 100) ?? 0)
               })
               .OrderBy(r => r.Year)
               .ThenBy(r => r.Month)
               .ToList();

            List<int> years = revenuePerDay.Select(r => r.Year).Distinct().ToList();

            ViewBag.Years = years;
            ViewBag.RevenueData = revenuePerDay;
            return View(Revenue);
        }



        public IActionResult support()
		{
			return View();
		}


	}
}
