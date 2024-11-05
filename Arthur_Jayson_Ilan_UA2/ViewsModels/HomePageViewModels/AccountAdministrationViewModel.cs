using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Dialogs.ViewModels;
using Arthur_Jayson_Ilan_UA2.Dialogs.Views;
using Arthur_Jayson_Ilan_UA2.Models;
using Arthur_Jayson_Ilan_UA2.Services;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.HomePageViewModels
{
    public class AccountAdministrationViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> _users = new ObservableCollection<User>();
        public ObservableCollection<User> Users
        {
            get => _users;
            set { _users = value; OnPropertyChanged(nameof(Users)); }
        }

        private User _selectedUser = new User();
        public User SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); }
        }

        // Commandes
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        // Pour afficher des notifications/messages
        private string _notificationMessage = string.Empty;
        public string NotificationMessage
        {
            get => _notificationMessage;
            set { _notificationMessage = value; OnPropertyChanged(nameof(NotificationMessage)); }
        }

        public AccountAdministrationViewModel()
        {
            Users = App.UserService.GetAllUsers();            
            EditUserCommand = new RelayCommand(EditUser, CanEditOrDelete);
            DeleteUserCommand = new RelayCommand(DeleteUser, CanEditOrDelete);

            // Souscrire à l'événement MessageSent pour afficher des notifications
            App.UserService.MessageSent += OnMessageSent;
        }

        private void EditUser(object? parameter)
        {
            if (parameter is User userToEdit)
            {
                // Implémenter la logique pour éditer l'utilisateur
                var editUserViewModel = new EditUserViewModel(userToEdit);
                var editUserView = new EditUserView
                {
                    DataContext = editUserViewModel
                };

                bool? result = editUserView.ShowDialog();
                if (result == true)
                {
                    // Actualiser la liste des utilisateurs si nécessaire
                    // Avec une ObservableCollection, les changements sont reflétés automatiquement
                    NotificationMessage = $"Utilisateur '{userToEdit.Username}' mis à jour avec succès.";
                }
            }
        }

        private bool CanEditOrDelete(object? parameter)
        {
            return parameter is User;
        }

        private void DeleteUser(object? parameter)
        {
            if (parameter is User userToDelete)
            {
                // Confirmer la suppression avec l'utilisateur
                var confirmation = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer l'utilisateur '{userToDelete.Username}' ?",
                    "Confirmer la suppression",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (confirmation == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Vous devez définir `currentUser` selon le contexte de votre application
                        // Par exemple, via un service d'authentification
                        User currentUser = GetCurrentUser(); // Méthode à implémenter
                        App.UserService.DeleteUser(currentUser, userToDelete);
                        NotificationMessage = $"Utilisateur '{userToDelete.Username}' supprimé avec succès.";
                    }
                    catch (Exception ex)
                    {
                        NotificationMessage = $"Erreur lors de la suppression de l'utilisateur : {ex.Message}";
                    }
                }
            }
        }

        private User GetCurrentUser()
        {
            // Implémenter la récupération de l'utilisateur actuellement connecté
            // Ceci est un placeholder
            return App.UserService.FindUserByUsername("admin") ?? throw new InvalidOperationException("Utilisateur admin non trouvé.");
        }

        private void OnMessageSent(object? sender, string message)
        {
            // Mettre à jour la notification ou afficher un message
            NotificationMessage = message;
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
