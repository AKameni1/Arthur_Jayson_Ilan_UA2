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
    [Table("ticketresponse")]
    public class TicketResponse : INotifyPropertyChanged
    {
        // Champs privés
        private int _responseID;
        private int _ticketID;
        private int _userID;
        private string _responseText = string.Empty;
        private DateTime _responseDate;

        // Propriétés publiques avec notifications de changement

        [Key]
        public int ResponseID
        {
            get => _responseID;
            set
            {
                if (_responseID != value)
                {
                    _responseID = value;
                    OnPropertyChanged(nameof(ResponseID));
                }
            }
        }

        [ForeignKey("SupportTicket")]
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

        public SupportTicket? SupportTicket { get; set; }

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

        public User? User { get; set; }

        [Required]
        public string ResponseText
        {
            get => _responseText;
            set
            {
                if (_responseText != value)
                {
                    _responseText = value;
                    OnPropertyChanged(nameof(ResponseText));
                }
            }
        }

        [Required]
        public DateTime ResponseDate
        {
            get => _responseDate;
            set
            {
                if (_responseDate != value)
                {
                    _responseDate = value;
                    OnPropertyChanged(nameof(ResponseDate));
                }
            }
        }

        // Constructeurs
        public TicketResponse() { }

        public TicketResponse(int responseID, int ticketID, int userID, string responseText)
        {
            ResponseID = responseID;
            TicketID = ticketID;
            UserID = userID;
            ResponseText = responseText;
            ResponseDate = DateTime.Now;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
