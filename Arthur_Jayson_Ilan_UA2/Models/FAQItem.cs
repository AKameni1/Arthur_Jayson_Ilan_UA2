using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public class FAQItem : INotifyPropertyChanged
    {
        private string _question = string.Empty;
        private string _answer = string.Empty;
        private bool _isExpanded;

        public string Question
        {
            get => _question;
            set { _question = value; OnPropertyChanged(nameof(Question)); }
        }

        public string Answer
        {
            get => _answer;
            set { _answer = value; OnPropertyChanged(nameof(Answer)); }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged(nameof(IsExpanded)); }
        }

        public FAQItem(string question, string answer)
        {
            Question = question;
            Answer = answer;
            IsExpanded = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
