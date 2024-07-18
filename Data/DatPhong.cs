using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class DatPhong
{
    public int IdDatPhong { get; set; }

    public int? UserId { get; set; }

    public int? IdPhong { get; set; }

    public double? TongTien { get; set; }

    public DateOnly? NgayCheckin { get; set; }

    public DateOnly? NgayCheckout { get; set; }

    public bool? TrangThai { get; set; }

    public virtual Phong? IdPhongNavigation { get; set; }

    public virtual User? User { get; set; }
}
