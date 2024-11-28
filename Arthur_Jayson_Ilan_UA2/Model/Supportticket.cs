using System;
using System.Collections.Generic;

namespace Arthur_Jayson_Ilan_UA2.Model;

public partial class Supportticket
{
    public int TicketId { get; set; }

    public int? UserId { get; set; }

    public string Subject { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Ticketresponse> Ticketresponses { get; set; } = new List<Ticketresponse>();

    public virtual User? User { get; set; }
}
