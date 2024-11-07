using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Dialogs.ViewModels;
using Arthur_Jayson_Ilan_UA2.Dialogs.Views;
using Arthur_Jayson_Ilan_UA2.Models;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.HomePageViewModels
{
    public class TechnicalAssitanceViewModel : INotifyPropertyChanged
    {


        // Propriétés pour le formulaire de soumission de ticket
        private string _userName = string.Empty;
        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(nameof(UserName)); }
        }

        private string _userEmail = string.Empty;
        public string UserEmail
        {
            get => _userEmail;
            set { _userEmail = value; OnPropertyChanged(nameof(UserEmail)); }
        }

        private string _subject = string.Empty;
        public string Subject
        {
            get => _subject;
            set { _subject = value; OnPropertyChanged(nameof(Subject)); }
        }

        private string _problemDescription = string.Empty;
        public string ProblemDescription
        {
            get => _problemDescription;
            set { _problemDescription = value; OnPropertyChanged(nameof(ProblemDescription)); }
        }

        private string _selectedFiles = string.Empty;
        public string SelectedFiles
        {
            get => _selectedFiles;
            set { _selectedFiles = value; OnPropertyChanged(nameof(SelectedFiles)); }
        }


        // Collection des tickets de l'utilisateur
        public ObservableCollection<Ticket> UserTickets { get; }
        public SnackbarMessageQueue MessageQueue { get; }

        // Commandes
        public ICommand BrowseFilesCommand { get; }
        public ICommand SubmitTicketCommand { get; }
        public ICommand ViewTicketCommand { get; }

        public TechnicalAssitanceViewModel()
        {
            // Initialisation des tickets
            UserTickets = new ObservableCollection<Ticket>();

            // Initialisation des commandes
            BrowseFilesCommand = new RelayCommand(BrowseFiles);
            SubmitTicketCommand = new RelayCommand(SubmitTicket, CanSubmitTicket);
            ViewTicketCommand = new RelayCommand(ViewTicket);

            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        }

        private void BrowseFiles(object? parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedFiles = string.Join(", ", openFileDialog.FileNames);
                // Ici, vous pouvez gérer les fichiers joints selon vos besoins (par exemple, les enregistrer temporairement)
            }
        }

        private void SubmitTicket(object? parameter)
        {
            // Création d'un nouveau ticket
            Ticket newTicket = new Ticket
            {
                UserName = this.UserName,
                UserEmail = this.UserEmail,
                Subject = this.Subject,
                ProblemDescription = this.ProblemDescription,
                FilePath = this.SelectedFiles
            };

            // Ajout du ticket à la collection
            UserTickets.Add(newTicket);

            // Réinitialisation des champs du formulaire
            UserName = string.Empty;
            UserEmail = string.Empty;
            Subject = string.Empty;
            ProblemDescription = string.Empty;
            SelectedFiles = string.Empty;

            //  système de notification ou de message box
            MessageQueue.Enqueue("Ticket soumis avec succès.");
        }

        private bool CanSubmitTicket(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(UserName) &&
                   !string.IsNullOrWhiteSpace(UserEmail) &&
                   IsValidEmail(UserEmail) &&
                   !string.IsNullOrWhiteSpace(Subject) &&
                   !string.IsNullOrWhiteSpace(ProblemDescription);
        }

        private void ViewTicket(object? parameter)
        {
            if (parameter is Ticket selectedTicket)
            {
                // Implémentez la logique pour afficher les détails du ticket
                // Par exemple, ouvrir une fenêtre de dialogue avec les informations du ticket

                // Implémenter la logique pour éditer l'utilisateur
                var showTicketViewModel = new ShowTicketViewModel(selectedTicket);
                var showTicketView = new ShowTicketView
                {
                    DataContext = showTicketViewModel
                };

                showTicketView.ShowDialog();
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
