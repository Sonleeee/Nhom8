using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class DichVu
{
    public int IdDichVu { get; set; }

    public string? TenDichVu { get; set; }

    public bool? TrangThai { get; set; }

    public int? IdKs { get; set; }

    public virtual KhachSan? IdKsNavigation { get; set; }
}
