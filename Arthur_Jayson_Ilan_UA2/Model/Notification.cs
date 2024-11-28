using System;
using System.Collections.Generic;

namespace Arthur_Jayson_Ilan_UA2.Model;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int? UserId { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? IsRead { get; set; }

    public virtual User? User { get; set; }
}
