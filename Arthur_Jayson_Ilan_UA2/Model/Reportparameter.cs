using System;
using System.Collections.Generic;

namespace Arthur_Jayson_Ilan_UA2.Model;

public partial class Reportparameter
{
    public int ParameterId { get; set; }

    public int? ReportId { get; set; }

    public string ParameterName { get; set; } = null!;

    public string ParameterValue { get; set; } = null!;

    public virtual Report? Report { get; set; }
}
