using System;
using System.Collections.Generic;

namespace Arthur_Jayson_Ilan_UA2.Model;

public partial class User
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? ProfileImage { get; set; }

    public int? IsActive { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? LoanCount { get; set; }

    public int? LoanLimit { get; set; }

    public int? LateReturnCount { get; set; }

    public int? PenaltyPoints { get; set; }

    public DateTime? LoanSuspendedUntil { get; set; }

    public virtual ICollection<Auditlog> Auditlogs { get; set; } = [];

    public virtual ICollection<Loan> Loans { get; set; } = [];

    public virtual ICollection<Notification> Notifications { get; set; } = [];

    public virtual ICollection<Report> Reports { get; set; } = [];

    public virtual ICollection<Reservation> Reservations { get; set; } = [];

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Supportticket> Supporttickets { get; set; } = [];

    public virtual ICollection<Ticketresponse> Ticketresponses { get; set; } = [];

    public virtual ICollection<Permission> Permissions { get; set; } = [];
}
