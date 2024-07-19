using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom8.Data;
using Nhom8.Areas.Hotel.Models;
using System.Runtime.Intrinsics.Arm;
namespace Nhom8_DACS.Areas.Hotel.Controllers
{

    [Area("Hotel")]
    public class HomeController : Controller
    {
        private readonly BookingHotelContext context;
        private readonly IWebHostEnvironment environment;

        public HomeController(BookingHotelContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        int userID = 1;

        public IActionResult Index()
        {
            ViewBag.ActivePage = "Index";
            var hotelInfo = context.KhachSans.Where(s => s.UserId == userID).ToList();
            return View(hotelInfo);
        }

        public IActionResult CustomerRating()
        {
            ViewBag.ActivePage = "CustomerRating";
            int? ksID = context.KhachSans
                               .Where(q => q.UserId.Equals(userID))
                               .Select(p => p.IdKs)
                               .FirstOrDefault();
            var comments = context.Comments
                                   .Where(dp => dp.KsId == ksID).Include(c => c.User)
                                   .ToList();
            return View(comments);
        }

        public IActionResult Revenue()
        {
            ViewBag.ActivePage = "Revenue";

            int? ksID = context.KhachSans
                               .Where(q => q.UserId.Equals(userID))
                               .Select(p => p.IdKs)
                               .Sum();

            if (ksID == null)
            {
                return NotFound();
            }

            int id = ksID.Value;

            // Lấy danh sách đặt phòng liên quan đến khách sạn
            var datPhongs = context.DatPhongs
                                   .Where(dp => dp.IdPhongNavigation.IdKs == id)
                                   .ToList();

            // Tính doanh thu theo từng ngày
            var revenuePerDay = datPhongs
                .Where(dp => dp.NgayCheckout.HasValue)
                .GroupBy(dp => new { dp.NgayCheckout.Value.Year, dp.NgayCheckout.Value.Month, dp.NgayCheckout.Value.Day })
                .Select(g => new RevenuePerDay
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Day = g.Key.Day,
                    Revenue = (float)(g.Sum(dp => (double?)dp.TongTien) ?? 0)
                })
                .OrderBy(r => r.Year)
                .ThenBy(r => r.Month)
                .ThenBy(r => r.Day)
                .ToList();

            // Trích xuất năm, tháng và ngày để truyền vào ViewBag
            List<int> years = revenuePerDay.Select(r => r.Year).Distinct().ToList();
            List<int> months = Enumerable.Range(1, 12).ToList(); // Tất cả 12 tháng

            ViewBag.Years = years;
            ViewBag.Months = months;
            ViewBag.RevenueData = revenuePerDay;

            return View(datPhongs);
        }



        public IActionResult RoomBooking(string date_type, DateOnly? fromDate, DateOnly? toDate)
        {
            ViewBag.ActivePage = "RoomBooking";

            int? ksID = context.KhachSans
                .Where(q => q.UserId.Equals(userID))
                .Select(p => p.IdKs)
                .FirstOrDefault();

            int id = ksID.Value;
            IQueryable<DatPhong> query = context.DatPhongs
                .Where(dp => dp.IdPhongNavigation.IdKs == id);

            // Apply filters based on date_type and date range
            if (!string.IsNullOrEmpty(date_type) && fromDate != null && toDate != null)
            {
                switch (date_type)
                {
                    case "booking":
                        query = query.Where(dp => dp.NgayCheckin >= fromDate && dp.NgayCheckin <= toDate);
                        break;
                    case "arrival":
                        query = query.Where(dp => dp.NgayCheckout >= fromDate && dp.NgayCheckout <= toDate);
                        break;
                    case "departure":
                        query = query.Where(dp => dp.TrangThai == false); 
                        break;
                    default:
                        break;
                }
            }
            else if(date_type == "departure")
                query = query.Where(dp => dp.TrangThai == false); 

            // Include related entities
            query = query.Include(dp => dp.User)
                         .Include(dp => dp.IdPhongNavigation);

            var datPhongs = query.ToList();
            return View(datPhongs);
        }
        public async Task<IActionResult> ThanhToan(int datPhongId)
        {
            var datPhong = await context.DatPhongs.FirstOrDefaultAsync(p => p.IdDatPhong == datPhongId);
            datPhong.TrangThai = true;
            context.DatPhongs.Update(datPhong);
            await context.SaveChangesAsync();
            return RedirectToAction("RoomBooking");
        }



        public IActionResult RoomInfo()
        {
            ViewBag.ActivePage = "RoomInfo";
            int? ksID = context.KhachSans
                 .Where(q => q.UserId.Equals(userID))
                 .Select(p => p.IdKs)
                 .FirstOrDefault();
            int id = ksID.Value;
            var Phong = context.Phongs.Where(p => p.IdKs == id).Include(h=>h.ImgRooms).Include(ct=>ct.ChiTietPhongs);
            return View(Phong.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom(string roomName, string roomType, float roomPrice, IFormFile[] roomImage, int roomBedNumber, float roomArea)
        {
            int? ksID = context.KhachSans
                  .Where(q => q.UserId.Equals(userID))
                  .Select(p => p.IdKs)
                  .FirstOrDefault();

            if (ksID == null)
            {
                return NotFound();
            }

            // Tạo một đối tượng phòng mới
            Phong phongMoi = new Phong()
            {
                TenPhong = roomName,
                LoaiPhong = roomType,
                GiaPhong = roomPrice,
                TinhTrangPhong = "trống",
                IdKs = ksID.Value,
                Hd = true,
            };

            context.Phongs.Add(phongMoi);
            await context.SaveChangesAsync();

            // Lưu hình ảnh vào thư mục wwwroot/assets/img/img_Phong
            foreach (var image in roomImage)
            {
                if (image != null && image.Length > 0)
                {
                    // Tạo tên tệp duy nhất để tránh xung đột
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(environment.WebRootPath, "assets", "img", "img_Phong", fileName);

                    // Tạo thư mục nếu chưa tồn tại
                    var directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Lưu hình ảnh vào thư mục tĩnh
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Tạo đối tượng ImgRoom và thêm vào cơ sở dữ liệu
                    var imgRoom = new ImgRoom
                    {
                        RoomId = phongMoi.IdPhong,
                        Img = "/assets/img/img_Phong/" + fileName
                    };

                    context.ImgRooms.Add(imgRoom);
                }
            }
            // chi tiết phòng mới
            ChiTietPhong chitietPhongMoi = new ChiTietPhong()
            {
                IdPhong = phongMoi.IdPhong,
                SlGiuong = roomBedNumber,
                DienTich = roomArea,
            };
            context.ChiTietPhongs.Add(chitietPhongMoi);
            await context.SaveChangesAsync();

            return RedirectToAction("RoomInfo");
        }




        public IActionResult Service(string searchString)
        {
            ViewBag.ActivePage = "Service";
            int? ksID = context.KhachSans
                  .Where(q => q.UserId.Equals(userID))
                  .Select(p => p.IdKs)
                  .FirstOrDefault();
            int id = ksID.Value;
            var DichVu = context.DichVus.Where(p => p.IdKs == id);

            if (!string.IsNullOrEmpty(searchString))
            {
                DichVu = DichVu.Where(d => d.TenDichVu.Contains(searchString));
            }
            return View(DichVu.ToList());
        }

        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddService(string tenDichVu)
        {

            int? ksID = context.KhachSans
                  .Where(q => q.UserId.Equals(userID))
                  .Select(p => p.IdKs)
                  .FirstOrDefault();

            DichVu dichVuMoi = new DichVu()
            {
                IdKs = ksID.Value,
                TenDichVu = tenDichVu,
                TrangThai = true,
            };
            context.DichVus.Add(dichVuMoi);
            context.SaveChanges();

            return RedirectToAction("Service");
        }
        public async Task<IActionResult> UpdateService(int id, bool trangThai)
        {
            var service = await context.DichVus.FirstOrDefaultAsync(p => p.IdDichVu == id);

            if (service == null)
            {
                return NotFound(); // Handle when service is not found
            }

            if(service.TrangThai == true)
                service.TrangThai = trangThai;
            else
                service.TrangThai = !trangThai;

            // Update the service in the database
            context.DichVus.Update(service);
            await context.SaveChangesAsync();

            // Redirect to the "Service" action method
            return RedirectToAction("Service");
        }
        public  IActionResult DeleteService(int id)
        {
            var dichVu =  context.DichVus.Find(id);
            
            context.DichVus.Remove(dichVu);
            context.SaveChanges();
            return RedirectToAction("Service");
        }

        public IActionResult TextTing()
        {
            ViewBag.ActivePage = "TextTing";
            return View();
        }
    }
}
