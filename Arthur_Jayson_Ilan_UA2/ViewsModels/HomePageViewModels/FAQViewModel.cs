using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.HomePageViewModels
{
    public class FAQViewModel : INotifyPropertyChanged
    {
        private string _question1Text = string.Empty;
        private string _question2Text = string.Empty;
        private string _question3Text = string.Empty;
        private string _question4Text = string.Empty;
        private string _question5Text = string.Empty;

        private bool _isQuestion1Expanded;
        private bool _isQuestion2Expanded;
        private bool _isQuestion3Expanded;
        private bool _isQuestion4Expanded;
        private bool _isQuestion5Expanded;

        public FAQViewModel()
        {
            // Initialisation des questions avec des réponses par défaut pour la FAQ
            Question1Text = "Pour utiliser l'application, veuillez suivre les instructions fournies sur la page d'accueil.";
            Question2Text = "Pour créer un compte, cliquez sur le bouton 'Inscription' et suivez les étapes indiquées.";
            Question3Text = "En cas de problème technique, contactez notre équipe support via le formulaire de contact.";
            Question4Text = "Pour réinitialiser votre mot de passe, cliquez sur le lien 'Mot de passe oublié' sur la page de connexion.";
            Question5Text = "Oui, vous pouvez supprimer votre compte en contactant le support via le formulaire de contact.";
        }

        public string Question1Text
        {
            get => _question1Text;
            set { _question1Text = value; OnPropertyChanged(); OnPropertyChanged(nameof(Question1Visibility)); }
        }

        public string Question2Text
        {
            get => _question2Text;
            set { _question2Text = value; OnPropertyChanged(); OnPropertyChanged(nameof(Question2Visibility)); }
        }

        public string Question3Text
        {
            get => _question3Text;
            set { _question3Text = value; OnPropertyChanged(); OnPropertyChanged(nameof(Question3Visibility)); }
        }

        public string Question4Text
        {
            get => _question4Text;
            set { _question4Text = value; OnPropertyChanged(); OnPropertyChanged(nameof(Question4Visibility)); }
        }

        public string Question5Text
        {
            get => _question5Text;
            set { _question5Text = value; OnPropertyChanged(); OnPropertyChanged(nameof(Question5Visibility)); }
        }

        public bool IsQuestion1Expanded
        {
            get => _isQuestion1Expanded;
            set { _isQuestion1Expanded = value; OnPropertyChanged(); }
        }

        public bool IsQuestion2Expanded
        {
            get => _isQuestion2Expanded;
            set { _isQuestion2Expanded = value; OnPropertyChanged(); }
        }

        public bool IsQuestion3Expanded
        {
            get => _isQuestion3Expanded;
            set { _isQuestion3Expanded = value; OnPropertyChanged(); }
        }

        public bool IsQuestion4Expanded
        {
            get => _isQuestion4Expanded;
            set { _isQuestion4Expanded = value; OnPropertyChanged(); }
        }

        public bool IsQuestion5Expanded
        {
            get => _isQuestion5Expanded;
            set { _isQuestion5Expanded = value; OnPropertyChanged(); }
        }

        public Visibility Question1Visibility => string.IsNullOrWhiteSpace(Question1Text) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility Question2Visibility => string.IsNullOrWhiteSpace(Question2Text) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility Question3Visibility => string.IsNullOrWhiteSpace(Question3Text) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility Question4Visibility => string.IsNullOrWhiteSpace(Question4Text) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility Question5Visibility => string.IsNullOrWhiteSpace(Question5Text) ? Visibility.Collapsed : Visibility.Visible;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
