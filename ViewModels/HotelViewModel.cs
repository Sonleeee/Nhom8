namespace Nhom8_DACS.ViewModels
{
    public class HotelViewModel
    {
        public int? ID_KS { get; set; }
        public string? TenKS { get; set; }
        public string? DiaChi { get; set; }
        public string? Tinh { get; set; }
        public string? DanhGia { get; set; }
        public string? Image_KS { get; set; }
        public string? TenPhong { get; set; }
        public double? GiaPhong { get; set; }
        public List<string>? TienNghi { get; set; }
    }
}
