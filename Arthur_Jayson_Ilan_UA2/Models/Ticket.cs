using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public class Ticket : INotifyPropertyChanged
    {
        private int _ticketId;
        private string _userName = string.Empty;
        private string _userEmail = string.Empty;
        private string _subject = string.Empty;
        private string _problemDescription = string.Empty;
        private string _status = string.Empty;
        private DateTime _submissionDate;
        private string _filePath = string.Empty;
        private ObservableCollection<Response> _responses = new ObservableCollection<Response>();


        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(nameof(UserName)); }
        }

        public string UserEmail
        {
            get => _userEmail;
            set { _userEmail = value; OnPropertyChanged(nameof(UserEmail)); }
        }

        public string Subject
        {
            get => _subject;
            set { _subject = value; OnPropertyChanged(nameof(Subject)); }
        }

        public string ProblemDescription
        {
            get => _problemDescription;
            set { _problemDescription = value; OnPropertyChanged(nameof(ProblemDescription)); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        public DateTime SubmissionDate
        {
            get => _submissionDate;
            set { _submissionDate = value; OnPropertyChanged(nameof(SubmissionDate)); }
        }

        public string FilePath
        {
            get => _filePath;
            set { _filePath = value; OnPropertyChanged(nameof(FilePath)); }
        }

        public ObservableCollection<Response> Responses
        {
            get => _responses;
            set { _responses = value; OnPropertyChanged(nameof(Responses)); }
        }

        public int TicketID
        {
            get => _ticketId;
            set { _ticketId = value; OnPropertyChanged(nameof(TicketID));}
        }

        public Ticket()
        {
            TicketID = IdGenerator.GetNextTicketId();
            SubmissionDate = DateTime.Now;
            Status = "Ouvert";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
