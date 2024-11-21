using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public enum AvailabilityStatus
    {
        Available,
        Borrowed,
        Reserved
    }

    [Table("book")]
    public class Book : INotifyPropertyChanged
    {
        // Champs privés
        private int _bookID;
        private string _title = string.Empty;
        private string _author = string.Empty;
        private string _isbn = string.Empty;
        private int? _publishedYear;
        private int? _categoryID;
        private AvailabilityStatus _availability;

        // Propriétés publiques avec notifications de changement

        [Key]
        public int BookID
        {
            get => _bookID;
            set
            {
                if (_bookID != value)
                {
                    _bookID = value;
                    OnPropertyChanged(nameof(BookID));
                }
            }
        }

        [Required]
        [MaxLength(255)]
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        [MaxLength(255)]
        public string Author
        {
            get => _author;
            set
            {
                if (_author != value)
                {
                    _author = value;
                    OnPropertyChanged(nameof(Author));
                }
            }
        }

        [Required]
        [MaxLength(20)]
        public string ISBN
        {
            get => _isbn;
            set
            {
                if (_isbn != value)
                {
                    _isbn = value;
                    OnPropertyChanged(nameof(ISBN));
                }
            }
        }

        public int? PublishedYear
        {
            get => _publishedYear;
            set
            {
                if (_publishedYear != value)
                {
                    _publishedYear = value;
                    OnPropertyChanged(nameof(PublishedYear));
                }
            }
        }

        [ForeignKey("Category")]
        public int? CategoryID
        {
            get => _categoryID;
            set
            {
                if (_categoryID != value)
                {
                    _categoryID = value;
                    OnPropertyChanged(nameof(CategoryID));
                }
            }
        }

        public Category? Category { get; set; }

        [Required]
        public AvailabilityStatus Availability
        {
            get => _availability;
            set
            {
                if (_availability != value)
                {
                    _availability = value;
                    OnPropertyChanged(nameof(Availability));
                }
            }
        }

        // Relations
        public ICollection<Loan>? Loans { get; }
        public ICollection<Reservation>? Reservations { get; }

        // Constructeurs
        public Book() { }

        public Book(int bookID, string title, string author, string isbn, int? publishedYear, int? categoryID, AvailabilityStatus availability = AvailabilityStatus.Available)
        {
            BookID = bookID;
            Title = title;
            Author = author;
            ISBN = isbn;
            PublishedYear = publishedYear;
            CategoryID = categoryID;
            Availability = availability;
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
