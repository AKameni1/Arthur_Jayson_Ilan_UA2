using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.HomePageViewModels
{
    public class TechnicalAssitanceViewModel : INotifyPropertyChanged
    {
        private string _question1Text = string.Empty;
        private string _question2Text = string.Empty;
        private string _question3Text = string.Empty;
        private string _question4Text = string.Empty;
        private string _question5Text = string.Empty;

        // Propriétés de texte pour chaque question
        public string Question1Text
        {
            get => _question1Text;
            set
            {
                _question1Text = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Question1Visibility)); // Actualise la visibilité
            }
        }

        public string Question2Text
        {
            get => _question2Text;
            set
            {
                _question2Text = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Question2Visibility)); // Actualise la visibilité
            }
        }

        public string Question3Text
        {
            get => _question3Text;
            set
            {
                _question3Text = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Question3Visibility)); // Actualise la visibilité
            }
        }

        public string Question4Text
        {
            get => _question4Text;
            set
            {
                _question4Text = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Question4Visibility)); // Actualise la visibilité
            }
        }

        public string Question5Text
        {
            get => _question5Text;
            set
            {
                _question5Text = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Question5Visibility)); // Actualise la visibilité
            }
        }

        // Propriétés de visibilité pour chaque question
        public Visibility Question1Visibility => string.IsNullOrWhiteSpace(Question1Text) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility Question2Visibility => string.IsNullOrWhiteSpace(Question2Text) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility Question3Visibility => string.IsNullOrWhiteSpace(Question3Text) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility Question4Visibility => string.IsNullOrWhiteSpace(Question4Text) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility Question5Visibility => string.IsNullOrWhiteSpace(Question5Text) ? Visibility.Collapsed : Visibility.Visible;

        // Autres méthodes
        public List<string> GetAssistanceMessages()
        {
            return new List<string>
            {
                "Comment résoudre un problème de connexion ?",
                "Que faire si l'application ne se charge pas ?",
                "Comment réinitialiser mon mot de passe ?"
            };
        }

        public bool IsAssistanceAvailable(string message)
        {
            return !string.IsNullOrWhiteSpace(message);
        }

        public void AddAssistanceMessage(string newMessage)
        {
            // Logique pour ajouter un message
        }

        public string GetAssistanceStatus()
        {
            return "L'assistance est en cours de traitement.";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
