
using Microsoft.AspNetCore.Mvc;

using Nhom8.Data;
using Nhom8.ViewModels;

namespace nhom8.viewcomponents
{
    public class FilterDichvuViewComponent : ViewComponent
    {
        private BookingHotelContext db;

        public FilterDichvuViewComponent(BookingHotelContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.DichVus.Select(dv => new FilterDichvuVM
            {
                MaDV = dv.IdDichVu,
                TenDV = dv.TenDichVu,
            }).OrderBy(t => t.TenDV);
            return View("FilterDichvu", data);
        }
    }
}
