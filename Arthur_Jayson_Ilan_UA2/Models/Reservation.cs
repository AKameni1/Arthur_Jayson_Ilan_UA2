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
    public enum ReservationStatus
    {
        Reserved,
        NotificationSent,
        Cancelled
    }

    public class Reservation : INotifyPropertyChanged
    {
        // Champs privés
        private int _reservationID;
        private int _userID;
        private int _bookID;
        private DateTime _reservationDate;
        private ReservationStatus _status;
        private DateTime _reservationEndDate;

        // Propriétés publiques avec notifications de changement

        [Key]
        public int ReservationID
        {
            get => _reservationID;
            set
            {
                if (_reservationID != value)
                {
                    _reservationID = value;
                    OnPropertyChanged(nameof(ReservationID));
                }
            }
        }

        [ForeignKey("User")]
        public int UserID
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

        public Book? Book { get; }

        [Required]
        public DateTime ReservationDate
        {
            get => _reservationDate;
            set
            {
                if (_reservationDate != value)
                {
                    _reservationDate = value;
                    OnPropertyChanged(nameof(ReservationDate));
                }
            }
        }

        [Required]
        public ReservationStatus Status
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

        public DateTime ReservationEndDate
        {
            get => _reservationEndDate;
            set
            {
                if (_reservationEndDate != value)
                {
                    _reservationEndDate = value;
                    OnPropertyChanged(nameof(ReservationEndDate));
                }
            }
        }

        // Constructeurs
        public Reservation() { }

        public Reservation(int reservationID, int userID, int bookID, DateTime? reservationEndDate = null, ReservationStatus status = ReservationStatus.Reserved)
        {
            ReservationID = reservationID;
            UserID = userID;
            BookID = bookID;
            ReservationDate = DateTime.Now;
            Status = status;
            ReservationEndDate = reservationEndDate ?? DateTime.Now.AddDays(3);
        }

        // Méthodes pour gérer le statut de la réservation
        public void CancelReservation()
        {
            Status = ReservationStatus.Cancelled;
            ReservationEndDate = DateTime.Now;
        }

        public void NotifyReservation()
        {
            Status = ReservationStatus.NotificationSent;
            // Logique d'envoi de notification ici
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
