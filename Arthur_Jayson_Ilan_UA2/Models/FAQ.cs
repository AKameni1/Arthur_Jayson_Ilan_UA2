using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public class FAQ : INotifyPropertyChanged
    {
        // Champs privés
        private int _faqID;
        private string _question = string.Empty;
        private string _answer = string.Empty;
        private DateTime _createdAt;
        private DateTime _updatedAt;

        // Propriétés publiques avec notifications de changement

        [Key]
        public int FAQID
        {
            get => _faqID;
            set
            {
                if (_faqID != value)
                {
                    _faqID = value;
                    OnPropertyChanged(nameof(FAQID));
                }
            }
        }

        [Required]
        public string Question
        {
            get => _question;
            set
            {
                if (_question != value)
                {
                    _question = value;
                    OnPropertyChanged(nameof(Question));
                }
            }
        }

        [Required]
        public string Answer
        {
            get => _answer;
            set
            {
                if (_answer != value)
                {
                    _answer = value;
                    OnPropertyChanged(nameof(Answer));
                }
            }
        }

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

        // Constructeurs
        public FAQ() { }

        public FAQ(int faqID, string question, string answer)
        {
            FAQID = faqID;
            Question = question;
            Answer = answer;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        // Méthodes pour mettre à jour les timestamps
        public void UpdateAnswer(string newAnswer)
        {
            Answer = newAnswer;
            UpdatedAt = DateTime.Now;
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
