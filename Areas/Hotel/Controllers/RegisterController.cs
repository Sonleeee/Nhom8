using Microsoft.AspNetCore.Mvc;

namespace Nhom8_DACS.Areas.Hotel.Controllers
{
    [Area("Hotel")]
    public class RegisterController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
