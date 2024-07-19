using Nhom8.Data;

namespace Nhom8.ViewModels
{
    public class DetailViewModel
    {
        public KhachSan _KhachSan { get; set; }
        public IEnumerable<Phong> _Phong { get; set; }
        public List<ChiTietPhong> _ChiTietPhongs { get; set; }
        public IEnumerable<TienNghi> _TienNghi { get; set; }
        public IEnumerable<DichVu> _DichVu { get; set; }
        public IEnumerable<ImgRoom> _ImgRoom { get; set; }
        public IEnumerable<QuyDinhChung> _QuyDinhChung { get; set; }
        public IEnumerable<Comment> _Comment { get; set; }

        public DetailViewModel()
        {
            _GroupedPhongs = new List<GroupedPhongViewModel>();
            _ChiTietPhongs = new List<ChiTietPhong>();
            _DichVu = new List<DichVu>();
        }
        public List<GroupedPhongViewModel> _GroupedPhongs { get; set; }
    }

    public class GroupedPhongViewModel
    {
        public string LoaiPhong { get; set; }
        public List<Phong> Phongs { get; set; }
    }
}
