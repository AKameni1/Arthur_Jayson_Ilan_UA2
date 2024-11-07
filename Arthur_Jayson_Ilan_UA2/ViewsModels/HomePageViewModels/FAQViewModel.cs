using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Models;
using MaterialDesignThemes.Wpf;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.HomePageViewModels
{
    public class FAQViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<FAQItem> FAQItems { get; set; }
        public SnackbarMessageQueue MessageQueue { get; }

        private string _newQuestion = string.Empty;
        public string NewQuestion
        {
            get => _newQuestion;
            set { _newQuestion = value; OnPropertyChanged(nameof(NewQuestion)); }
        }

        private string _newAnswer = string.Empty;
        public string NewAnswer
        {
            get => _newAnswer;
            set { _newAnswer = value; OnPropertyChanged(nameof(NewAnswer)); }
        }

        public ICommand AddFAQCommand { get; }


        public FAQViewModel()
        {
            // Initialisation des questions avec des réponses par défaut pour la FAQ
            FAQItems =
            [
                new(
                    "Comment emprunter un livre ?",
                    "Pour emprunter un livre, connectez-vous à votre compte, recherchez le livre souhaité et cliquez sur 'Emprunter'. Vous pourrez ensuite le récupérer à la bibliothèque ou le recevoir par courrier si disponible."
                ),
                new(
                    "Quels sont les horaires d'ouverture de la bibliothèque ?",
                    "La bibliothèque est ouverte du lundi au vendredi de 9h à 18h, et le samedi de 10h à 16h. Fermée les dimanches et jours fériés."
                ),
                new(
                    "Comment réserver un livre en ligne ?",
                    "Pour réserver un livre, recherchez-le dans notre catalogue en ligne, puis cliquez sur 'Réserver'. Vous recevrez une confirmation par e-mail dès que le livre sera disponible."
                ),
                new(
                    "Puis-je renouveler mon emprunt en ligne ?",
                    "Oui, vous pouvez renouveler votre emprunt en vous connectant à votre compte et en accédant à la section 'Emprunts'."
                ),
                new(
                    "Que faire en cas de perte ou de dommage d'un livre emprunté ?",
                    "Si vous perdez ou endommagez un livre, veuillez contacter le support via le formulaire de contact dans l'application ou par email à support@librarymanagement.com."
                ),
                new(
                    "Comment accéder aux ressources numériques de la bibliothèque ?",
                    "Pour accéder aux ebooks et aux bases de données, connectez-vous à votre compte et allez dans la section 'Ressources Numériques'."
                ),
                new(
                    "Comment mettre à jour mes informations personnelles ?",
                    "Allez dans les paramètres de votre compte, cliquez sur 'Informations Personnelles' et mettez à jour vos données. N'oubliez pas de sauvegarder vos modifications."
                )
            ];

            AddFAQCommand = new RelayCommand(AddFAQ, CanAddFAQ);
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        }

        private void AddFAQ(object? parameter)
        {
            if (!string.IsNullOrWhiteSpace(NewQuestion) && !string.IsNullOrWhiteSpace(NewAnswer))
            {
                FAQItems.Add(new FAQItem(NewQuestion, NewAnswer));
                NewQuestion = string.Empty;
                NewAnswer = string.Empty;

                MessageQueue.Enqueue("Nouvelle question ajoutée avec succès.");
            }
        }

        private bool CanAddFAQ(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(NewQuestion) && !string.IsNullOrWhiteSpace(NewAnswer);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
