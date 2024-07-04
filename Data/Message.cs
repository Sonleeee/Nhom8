using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class Message
{
    public int MessageId { get; set; }

    public int? ConversationId { get; set; }

    public int? SenderId { get; set; }

    public string MessageContent { get; set; } = null!;

    public int? UserId { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual Conversation? Conversation { get; set; }

    public virtual User? User { get; set; }
}
