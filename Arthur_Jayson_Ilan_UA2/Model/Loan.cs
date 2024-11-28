using System;
using System.Collections.Generic;

namespace Arthur_Jayson_Ilan_UA2.Model;

public partial class Loan
{
    public int LoanId { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string? Status { get; set; }

    public int? DueNotificationSent { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
