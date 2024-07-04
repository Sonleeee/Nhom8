using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class QuyDinhChung
{
    public int IdQuyDinh { get; set; }

    public string? TenQuyDinh { get; set; }

    public bool? TrangThai { get; set; }

    public int? IdKs { get; set; }

    public virtual KhachSan? IdKsNavigation { get; set; }
}
