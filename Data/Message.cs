using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class Message
{
    public int MessageId { get; set; }

    public int? SenderId { get; set; }

    public int ConVid { get; set; }

    public string MessageContent { get; set; } = null!;

    public int? UserId { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual Conversation ConV { get; set; } = null!;
}
