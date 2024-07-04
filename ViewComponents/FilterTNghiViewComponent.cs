using Microsoft.AspNetCore.Mvc;

using Nhom8_DACS.ViewModels;
using System.ComponentModel;

namespace Nhom8_DACS.ViewComponents
{
    public class FilterTNghiViewComponent : ViewComponent
    {
        //private BookingHotelContext db;

        //public FilterTNghiViewComponent(BookingHotelContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            //var data = db.TienNghis.Select(tiennghi => new FilterTNghiVM
            //{
            //    MaTN = tiennghi.MaTienNghi,
            //    DieuHoa = tiennghi.DieuHoa.GetValueOrDefault(),
            //    HeThongSuoi = tiennghi.HeThongSuoi.GetValueOrDefault(),
            //    Bep = tiennghi.Bep.GetValueOrDefault(),
            //    MayGiac = tiennghi.MayGiac.GetValueOrDefault(),
            //    TV = tiennghi.Tv.GetValueOrDefault(),
            //    HoBoi = tiennghi.HoBoi.GetValueOrDefault(),
            //    MiniBar = tiennghi.MiniBar.GetValueOrDefault(),
            //    BanCong = tiennghi.BanCong.GetValueOrDefault(),
            //    SanThuong = tiennghi.SanThuong.GetValueOrDefault(),
            //});            
            //return View("FilterTNghi", data);
            return View();
        }
    }
}
