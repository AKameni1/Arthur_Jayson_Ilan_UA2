using System;
using System.Collections.Generic;

namespace Arthur_Jayson_Ilan_UA2.Model;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string? Author { get; set; }

    public string Isbn { get; set; } = null!;

    public int? PublishedYear { get; set; }

    public int? CategoryId { get; set; }

    public string? Availability { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
