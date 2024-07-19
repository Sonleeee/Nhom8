using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class KhachSan
{
    public int? UserId { get; set; }

    public int IdKs { get; set; }

    public string? TenKs { get; set; }

    public string? DiaChi { get; set; }

    public string? MoTa { get; set; }

    public double? Star { get; set; }

    public int? TinhId { get; set; }

    public string? ImageKs { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<DichVu> DichVus { get; set; } = new List<DichVu>();

    public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();

    public virtual ICollection<QuyDinhChung> QuyDinhChungs { get; set; } = new List<QuyDinhChung>();

    public virtual Tinh? Tinh { get; set; }

    public virtual User? User { get; set; }
}
