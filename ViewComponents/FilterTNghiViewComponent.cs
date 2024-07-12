using Microsoft.AspNetCore.Mvc;
using Nhom8.Data;
using Nhom8.ViewModels;
using System.ComponentModel;

namespace Nhom8.ViewComponents
{
    public class FilterTNghiViewComponent : ViewComponent
    {
        private BookingHotelContext db;

        public FilterTNghiViewComponent(BookingHotelContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.TienNghis.Select(tiennghi => new FilterTNghiVM
            {
                MaTN = tiennghi.MaTienNghi,
                TenTN = tiennghi.TenTienNghi,
            }).OrderBy(t => t.TenTN);
            return View("FilterTNghi", data);
        }
    }
}
