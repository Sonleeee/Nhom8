using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class TienNghi
{
    public int MaTienNghi { get; set; }

    public string? TenTienNghi { get; set; }

    public bool? TrangThai { get; set; }

    public int? IdChiTietPhong { get; set; }

    public virtual ChiTietPhong? IdChiTietPhongNavigation { get; set; }
}
