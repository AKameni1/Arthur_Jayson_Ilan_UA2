using System;
using System.Collections.Generic;

namespace Arthur_Jayson_Ilan_UA2.Model;

public partial class Ticketresponse
{
    public int ResponseId { get; set; }

    public int? TicketId { get; set; }

    public int? UserId { get; set; }

    public string ResponseText { get; set; } = null!;

    public DateTime? ResponseDate { get; set; }

    public virtual Supportticket? Ticket { get; set; }

    public virtual User? User { get; set; }
}
