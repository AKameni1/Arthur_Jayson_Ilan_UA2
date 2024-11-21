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
    public enum TicketStatus
    {
        Open,
        InProgress,
        Closed
    }

    [Table("supportticket")]
    public class SupportTicket : INotifyPropertyChanged
    {

        // Champs privés
        private int _ticketID;
        private int? _userID;
        private string _subject = string.Empty;
        private string _description = string.Empty;
        private TicketStatus _status;
        private DateTime _createdAt;
        private DateTime _updatedAt;

        // Propriétés publiques avec notifications de changement

        [Key]
        public int TicketID
        {
            get => _ticketID;
            set
            {
                if (_ticketID != value)
                {
                    _ticketID = value;
                    OnPropertyChanged(nameof(TicketID));
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

        public User? User { get; set; }

        [Required]
        [MaxLength(255)]
        public string Subject
        {
            get => _subject;
            set
            {
                if (_subject != value)
                {
                    _subject = value;
                    OnPropertyChanged(nameof(Subject));
                }
            }
        }

        [Required]
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        [Required]
        public TicketStatus Status
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

        [Required]
        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                if (_createdAt != value)
                {
                    _createdAt = value;
                    OnPropertyChanged(nameof(CreatedAt));
                }
            }
        }

        [Required]
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set
            {
                if (_updatedAt != value)
                {
                    _updatedAt = value;
                    OnPropertyChanged(nameof(UpdatedAt));
                }
            }
        }

        // Relations
        public ICollection<TicketResponse>? TicketResponses { get; set; }

        // Constructeurs
        public SupportTicket() { }

        public SupportTicket(int ticketID, int? userID, string subject, string description, TicketStatus status = TicketStatus.Open)
        {
            TicketID = ticketID;
            UserID = userID;
            Subject = subject;
            Description = description;
            Status = status;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        // Méthodes pour gérer le statut du ticket
        public void UpdateStatus(TicketStatus newStatus)
        {
            Status = newStatus;
            UpdatedAt = DateTime.Now;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
