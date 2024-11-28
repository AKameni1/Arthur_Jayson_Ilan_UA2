using System;
using System.Collections.Generic;

namespace Arthur_Jayson_Ilan_UA2.Model;

public partial class Faq
{
    public int Faqid { get; set; }

    public string Question { get; set; } = null!;

    public string Answer { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
