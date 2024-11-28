using System;
using System.Collections.Generic;

namespace Arthur_Jayson_Ilan_UA2.Model;

public partial class Auditlog
{
    public int AuditId { get; set; }

    public int? UserId { get; set; }

    public string? Entity { get; set; }

    public string Action { get; set; } = null!;

    public string? Details { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual User? User { get; set; }
}
