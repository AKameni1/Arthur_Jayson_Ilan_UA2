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
    public enum LoanStatus
    {
        Borrowed,
        Returned,
        Late
    }

    [Table("loan")]
    public class Loan : INotifyPropertyChanged
    {

        // Champs privés
        private int _loanID;
        private int? _userID;
        private int? _bookID;
        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime? _returnDate;
        private LoanStatus _status;
        private bool _dueNotificationSent;

        // Propriétés publiques avec notifications de changement

        [Key]
        public int LoanID
        {
            get => _loanID;
            set
            {
                if (_loanID != value)
                {
                    _loanID = value;
                    OnPropertyChanged(nameof(LoanID));
                }
            }
        }

        [ForeignKey("User")]
        public int? UserID
        {
            get => _userID;
            set
            {
                if (_userID != value)
                {
                    _userID = value;
                    OnPropertyChanged(nameof(UserID));
                }
            }
        }

        public User? User { get; }

        [ForeignKey("Book")]
        public int? BookID
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

        public Book? Book { get; }

        [Required]
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        [Required]
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        public DateTime? ReturnDate
        {
            get => _returnDate;
            set
            {
                if (_returnDate != value)
                {
                    _returnDate = value;
                    OnPropertyChanged(nameof(ReturnDate));
                }
            }
        }

        [Required]
        public LoanStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public bool DueNotificationSent
        {
            get => _dueNotificationSent;
            set
            {
                if (_dueNotificationSent != value)
                {
                    _dueNotificationSent = value;
                    OnPropertyChanged(nameof(DueNotificationSent));
                }
            }
        }

        // Constructeurs
        public Loan() { }

        public Loan(int loanID, int? userID, int? bookID, DateTime? endDate = null, LoanStatus status = LoanStatus.Borrowed, bool dueNotificationSent = false)
        {
            LoanID = loanID;
            UserID = userID;
            BookID = bookID;
            StartDate = DateTime.Now;
            EndDate = endDate ?? DateTime.Now.AddDays(7);
            Status = status;
            DueNotificationSent = dueNotificationSent;
        }

        // Méthodes pour gérer le retour d'un livre
        public void ReturnBook()
        {
            ReturnDate = DateTime.Now;
            Status = LoanStatus.Returned;
            // Logique additionnelle si nécessaire
        }



        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
