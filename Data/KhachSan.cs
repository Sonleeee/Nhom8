using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class KhachSan
{
    public int? UserId { get; set; }

    public int IdKs { get; set; }

    public string? TenKs { get; set; }

    public string? DiaChi { get; set; }

    public string? DanhGia { get; set; }

    public string? Tinh { get; set; }

    public string? ImageKs { get; set; }

    public virtual ICollection<DichVu> DichVus { get; set; } = new List<DichVu>();

    public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();

    public virtual ICollection<QuyDinhChung> QuyDinhChungs { get; set; } = new List<QuyDinhChung>();

    public virtual User? User { get; set; }
}
