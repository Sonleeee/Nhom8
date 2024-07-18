using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class User
{
    public int UserId { get; set; }

    public string? Role { get; set; }

    public string? Tk { get; set; }

    public string? Mk { get; set; }

    public string? TenKh { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public int? Otp { get; set; }

    public string? Img { get; set; }

    public string? RandomKey { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

    public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();

    public virtual ICollection<KhachSan> KhachSans { get; set; } = new List<KhachSan>();
}
