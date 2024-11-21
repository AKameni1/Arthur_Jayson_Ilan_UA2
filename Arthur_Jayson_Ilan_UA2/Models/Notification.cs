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
    [Table("notification")]
    public class Notification : INotifyPropertyChanged
    {

        // Champs privés
        private int _notificationID;
        private int? _userID;
        private string _message = string.Empty;
        private DateTime _createdAt;
        private bool _isRead;

        // Propriétés publiques avec notifications de changement

        [Key]
        public int NotificationID
        {
            get => _notificationID;
            set
            {
                if (_notificationID != value)
                {
                    _notificationID = value;
                    OnPropertyChanged(nameof(NotificationID));
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

        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged(nameof(Message));
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

        public bool IsRead
        {
            get => _isRead;
            set
            {
                if (_isRead != value)
                {
                    _isRead = value;
                    OnPropertyChanged(nameof(IsRead));
                }
            }
        }

        // Constructeurs
        public Notification() { }

        public Notification(int notificationID, int? userID, string message, DateTime? createdAt = null, bool isRead = false)
        {
            NotificationID = notificationID;
            UserID = userID;
            Message = message;
            CreatedAt = createdAt ?? DateTime.Now;
            IsRead = isRead;
        }

        // Méthodes pour marquer comme lu
        public void MarkAsRead()
        {
            IsRead = true;
        }


        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
