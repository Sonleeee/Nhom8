using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class ChiTietPhong
{
    public int IdChiTietPhong { get; set; }

    public int? IdPhong { get; set; }

    public double? DienTich { get; set; }

    public int? SlGiuong { get; set; }

    public virtual Phong? IdPhongNavigation { get; set; }

    public virtual ICollection<TienNghi> TienNghis { get; set; } = new List<TienNghi>();
}
