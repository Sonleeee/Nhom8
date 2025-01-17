﻿using System;
using System.Collections.Generic;

namespace Nhom8.Data;

public partial class Conversation
{
    public int ConversationId { get; set; }

    public int? UserId { get; set; }

    public DateTime StartTime { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual User? User { get; set; }
}
