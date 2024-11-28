using System;
using System.Collections.Generic;

namespace Arthur_Jayson_Ilan_UA2.Model;

public partial class Report
{
    public int ReportId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? GeneratedBy { get; set; }

    public DateTime? GeneratedAt { get; set; }

    public string? ReportPath { get; set; }

    public string ReportType { get; set; } = null!;

    public virtual User? GeneratedByNavigation { get; set; }

    public virtual ICollection<Reportparameter> Reportparameters { get; set; } = new List<Reportparameter>();
}
