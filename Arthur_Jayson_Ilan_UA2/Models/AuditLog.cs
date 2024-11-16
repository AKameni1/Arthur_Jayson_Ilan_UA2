using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public class AuditLog : INotifyPropertyChanged
    {
        // Champs privés
        private int _auditID;
        private int? _userID;
        private string _entity = string.Empty;
        private string _action = string.Empty;
        private DateTime _timestamp;
        private string _details = string.Empty;

        // Propriétés publiques avec notifications de changement

        [Key]
        public int AuditID
        {
            get => _auditID;
            set
            {
                if (_auditID != value)
                {
                    _auditID = value;
                    OnPropertyChanged(nameof(AuditID));
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

        [MaxLength(100)]
        public string Entity
        {
            get => _entity;
            set
            {
                if (_entity != value)
                {
                    _entity = value;
                    OnPropertyChanged(nameof(Entity));
                }
            }
        }

        [Required]
        [MaxLength(255)]
        public string Action
        {
            get => _action;
            set
            {
                if (_action != value)
                {
                    _action = value;
                    OnPropertyChanged(nameof(Action));
                }
            }
        }

        [Required]
        public DateTime Timestamp
        {
            get => _timestamp;
            set
            {
                if (_timestamp != value)
                {
                    _timestamp = value;
                    OnPropertyChanged(nameof(Timestamp));
                }
            }
        }

        public string Details
        {
            get => _details;
            set
            {
                if (_details != value)
                {
                    _details = value;
                    OnPropertyChanged(nameof(Details));
                }
            }
        }

        // Constructeurs
        public AuditLog() { }

        public AuditLog(int auditID, int? userID, string entity, string action, string details, DateTime? timestamp = null)
        {
            AuditID = auditID;
            UserID = userID;
            Entity = entity;
            Action = action;
            Timestamp = timestamp ?? DateTime.Now;
            Details = details;
        }



        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
