using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Commands;
using System.Windows.Input;
using System.Windows;
using Arthur_Jayson_Ilan_UA2.Models;

namespace Arthur_Jayson_Ilan_UA2.Dialogs.ViewModels
{
    public class ShowTicketViewModel : INotifyPropertyChanged
    {
        private readonly Ticket _ticket;

        public ShowTicketViewModel(Ticket ticketToView)
        {
            _ticket = ticketToView;
        }

        // Propriétés nécessaires pour les Bindings dans la Vue
        public string TicketId => _ticket.TicketID.ToString();

        public string Title => _ticket.Subject;
        public string Description => _ticket.ProblemDescription;
        public DateTime CreationDate => _ticket.SubmissionDate;
        public string User => _ticket.UserName;


        public string UserName
        {
            get => _ticket.UserName;
            set
            {
                if (_ticket.UserName != value)
                {
                    _ticket.UserName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        public string UserEmail
        {
            get => _ticket.UserEmail;
            set
            {
                if (_ticket.UserEmail != value)
                {
                    _ticket.UserEmail = value;
                    OnPropertyChanged(nameof(UserEmail));
                }
            }
        }

        public string Subject
        {
            get => _ticket.Subject;
            set
            {
                if (_ticket.Subject != value)
                {
                    _ticket.Subject = value;
                    OnPropertyChanged(nameof(Subject));
                }
            }
        }

        public string ProblemDescription
        {
            get => _ticket.ProblemDescription;
            set
            {
                if (_ticket.ProblemDescription != value)
                {
                    _ticket.ProblemDescription = value;
                    OnPropertyChanged(nameof(ProblemDescription));
                }
            }
        }

        public string Status
        {
            get => _ticket.Status;
            set
            {
                if (_ticket.Status != value)
                {
                    _ticket.Status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public DateTime SubmissionDate
        {
            get => _ticket.SubmissionDate;
            set
            {
                if (_ticket.SubmissionDate != value)
                {
                    _ticket.SubmissionDate = value;
                    OnPropertyChanged(nameof(SubmissionDate));
                }
            }
        }

        public string FilePath
        {
            get => _ticket.FilePath;
            set
            {
                if (_ticket.FilePath != value)
                {
                    _ticket.FilePath = value;
                    OnPropertyChanged(nameof(FilePath));
                }
            }
        }

        public ObservableCollection<Response> Responses => _ticket.Responses;

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Commande pour annuler l'affichage
        private ICommand? _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ??= new RelayCommand<Window>(ExecuteCancelCommand);

        private void ExecuteCancelCommand(Window? window)
        {
            window?.Close();
        }
    }
}
