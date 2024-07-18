using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class Comment
{
    public int CmId { get; set; }

    public int UserId { get; set; }

    public int KsId { get; set; }

    public string? CmContent { get; set; }

    public double? Star { get; set; }

    public DateTime TimeCm { get; set; }

    public virtual KhachSan Ks { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
