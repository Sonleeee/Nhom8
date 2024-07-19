using Microsoft.AspNetCore.Mvc;
using Nhom8.Data;

namespace Nhom8_DACS.Areas.Hotel.Controllers
{
    [Area("Hotel")]
    public class RegisterController : Controller
    {
        private readonly BookingHotelContext context;
        private readonly IWebHostEnvironment environment;

        public RegisterController(BookingHotelContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        int userID = 5;
        public IActionResult Register()
        {
            var Tinhs = context.Tinhs.ToList();
            //Console.WriteLine($"Số lượng tỉnh: {Tinhs.Count}"); // In số lượng tỉnh ra console hoặc log để kiểm tra
            ViewBag.Tinhs = Tinhs;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(string tenKs, string diaChi, int tinhID, IFormFile[] hinhKS)
        {
            
             // Lưu hình ảnh vào thư mục wwwroot/assets/img/img_Phong
            foreach (var image in hinhKS)
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

                    // Tạo đối tượng khách sạn và thêm vào cơ sở dữ liệu
                    KhachSan khachSanMoi = new KhachSan()
                    {
                        UserId = userID,
                        TenKs = tenKs,
                        DiaChi = diaChi,
                        TinhId = tinhID,
                        ImageKs = "/assets/img/img_Phong/" + fileName
                    };

                    context.KhachSans.Add(khachSanMoi);
                }
                var user = context.Users.Where(u=>u.UserId == userID).FirstOrDefault();
                user.Role = "KS";
                
            }
            await context.SaveChangesAsync();
            return View();
        }
    }
}
