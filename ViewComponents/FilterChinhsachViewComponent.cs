using Microsoft.AspNetCore.Mvc;
using Nhom8.Data;
using Nhom8.ViewModels;

namespace Nhom8.ViewComponents
{
    public class FilterChinhsachViewComponent : ViewComponent
    {
        private BookingHotelContext db;

        public FilterChinhsachViewComponent(BookingHotelContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.QuyDinhChungs.Select(cs => new FilterChinhsachVM
            {
                MaCS = cs.IdQuyDinh,
                TenCS = cs.TenQuyDinh,
            }).OrderBy(t => t.TenCS);
            return View("FilterChinhsach", data);
        }
    }
}
