using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class ImgRoom
{
    public int ImgId { get; set; }

    public int? RoomId { get; set; }

    public string Img { get; set; } = null!;

    public virtual Phong? Room { get; set; }
}
