using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public class Response : INotifyPropertyChanged
    {
        private string _responseText = string.Empty;
        private string _respondent = string.Empty; // Nom de la personne qui a répondu (support ou utilisateur)
        private DateTime _responseDate;

        public string ResponseText
        {
            get => _responseText;
            set { _responseText = value; OnPropertyChanged(nameof(ResponseText)); }
        }

        public string Respondent
        {
            get => _respondent;
            set { _respondent = value; OnPropertyChanged(nameof(Respondent)); }
        }

        public DateTime ResponseDate
        {
            get => _responseDate;
            set { _responseDate = value; OnPropertyChanged(nameof(ResponseDate)); }
        }

        public Response()
        {
            ResponseDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
