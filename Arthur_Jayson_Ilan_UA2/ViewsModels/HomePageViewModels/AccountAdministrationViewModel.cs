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
using MaterialDesignThemes.Wpf;

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

        public SnackbarMessageQueue MessageQueue { get; }

        private User _selectedUser = new User();
        public User SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); }
        }

        public static bool IsCurrentUserSuperAdmin => App.UserService.CurrentUser?.Role == UserRole.SuperAdmin;

        // Commandes
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand DeleteMultipleUsersCommand { get; }

        public AccountAdministrationViewModel()
        {
            Users = App.UserService.GetAllUsers();

            EditUserCommand = new RelayCommand(EditUser, CanEditOrDelete);
            DeleteUserCommand = new RelayCommand(DeleteUser, CanEditOrDelete);
            DeleteMultipleUsersCommand = new RelayCommand(DeleteMultipleUsers, CanDeleteMultipleUsers);

            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

            // Souscrire à l'événement MessageSent pour afficher des notifications
            App.UserService.MessageSent += OnMessageSent;

            // Souscrire aux changements de sélection des utilisateurs
            foreach (var user in Users)
            {
                user.PropertyChanged += User_PropertyChanged;
            }
        }

        private void User_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(User.IsSelected))
            {
                OnPropertyChanged(nameof(SelectedUsersCount));
                OnPropertyChanged(nameof(CanShowDeleteMultipleButton));
            }
        }

        // Propriété pour compter les utilisateurs sélectionnés
        public int SelectedUsersCount => Users.Count(u => u.IsSelected && u.Role != UserRole.SuperAdmin);

        // Propriété pour déterminer la visibilité du bouton de suppression multiple
        public bool CanShowDeleteMultipleButton => SelectedUsersCount >= 2 && IsCurrentUserSuperAdmin;

        // Commande de suppression multiple
        private void DeleteMultipleUsers(object? parameter)
        {
            var usersToDelete = Users.Where(u => u.IsSelected && u.Role != UserRole.SuperAdmin).ToList();

            if (usersToDelete.Count < 2)
            {
                MessageQueue.Enqueue("Sélectionnez au moins deux utilisateurs à supprimer.");
                return;
            }

            // Confirmer la suppression
            var confirmation = MessageBox.Show(
                $"Êtes-vous sûr de vouloir supprimer {usersToDelete.Count} utilisateurs sélectionnés ?",
                "Confirmer la suppression multiple",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmation == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var user in usersToDelete)
                    {
                        App.UserService.DeleteUser(App.UserService.CurrentUser, user);
                    }
                    MessageQueue.Enqueue($"{usersToDelete.Count} utilisateurs supprimés avec succès.");
                    // Rafraîchir la liste des utilisateurs
                    //Users = App.UserService.GetAllUsers();
                }
                catch (Exception ex)
                {
                    MessageQueue.Enqueue($"Erreur lors de la suppression : {ex.Message}");
                }
            }
        }

        private bool CanDeleteMultipleUsers(object? parameter)
        {
            return CanShowDeleteMultipleButton;
        }

        private void EditUser(object? parameter)
        {
            if (parameter is User userToEdit)
            {
                // Vérifier les permissions
                if (App.UserService.CurrentUser == null || (!App.UserService.CurrentUser.IsSuperAdmin && !App.UserService.CurrentUser.IsAdmin()))
                {
                    MessageQueue.Enqueue("Vous n'avez pas la permission d'éditer les utilisateurs.");
                    return;
                }

                // Implémenter la logique pour éditer l'utilisateur
                var editUserViewModel = new EditUserViewModel(userToEdit);
                var editUserView = new EditUserView
                {
                    DataContext = editUserViewModel
                };

                bool? result = editUserView.ShowDialog();
                if (result == true)
                {
                    MessageQueue.Enqueue($"Utilisateur '{userToEdit.Username}' mis à jour avec succès.");
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
                // Vérifier les permissions
                if (App.UserService.CurrentUser == null || (!App.UserService.CurrentUser.IsSuperAdmin && !App.UserService.CurrentUser.IsAdmin()))
                {                    
                    MessageQueue.Enqueue("Vous n'avez pas la permission de supprimer les utilisateurs.");
                    return;
                }

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
                        App.UserService.DeleteUser(App.UserService.CurrentUser, userToDelete);
                        MessageQueue.Enqueue($"Utilisateur '{userToDelete.Username}' supprimé avec succès.");                        

                        if (userToDelete == App.UserService.CurrentUser)
                        {
                            App.UserService.Logout();

                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();

                            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
                        }
                    }
                    catch (Exception ex)
                    {                        
                        MessageQueue.Enqueue($"Erreur lors de la suppression de l'utilisateur : {ex.Message}");
                    }
                }
            }
        }

        private void OnMessageSent(object? sender, string message)
        {
            // Mettre à jour la notification ou afficher un message
            MessageQueue.Enqueue(message);
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
