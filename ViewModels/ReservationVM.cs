using Nhom8.Data;

namespace Nhom8.ViewModels
{
    public class ReservationVM
    {
        public string Name { get; set; }    
        public string Email { get; set; }
        public string Sdt { get; set; }
        public KhachSanPhongViewModel KhachSanPhong { get; set; }
        public ChiTietPhong ChiTietPhong { get; set; }
        public string CheckinDate {  get; set; }
        public string CheckoutDate { get; set;}
        public int Price { get; set; }
    }

    public class KhachSanPhongViewModel
    {
        public KhachSan KhachSan { get; set; }
        public Phong Phong { get; set; }
    }
}
