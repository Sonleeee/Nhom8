﻿using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class Phong
{
    public int IdPhong { get; set; }

    public int? IdKs { get; set; }

    public string? TenPhong { get; set; }

    public string? LoaiPhong { get; set; }

    public string? TinhTrangPhong { get; set; }

    public double? GiaPhong { get; set; }

    public bool? Hd { get; set; }

    public virtual ICollection<ChiTietPhong> ChiTietPhongs { get; set; } = new List<ChiTietPhong>();

    public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();

    public virtual KhachSan? IdKsNavigation { get; set; }

    public virtual ICollection<ImgRoom> ImgRooms { get; set; } = new List<ImgRoom>();
}
