using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class Tinh
{
    public int IdTinh { get; set; }

    public string? Tinh1 { get; set; }

    public string? ImgTinh { get; set; }

    public virtual ICollection<KhachSan> KhachSans { get; set; } = new List<KhachSan>();
}
