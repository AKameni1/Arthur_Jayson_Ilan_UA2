using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Models;
using Arthur_Jayson_Ilan_UA2.Services;

namespace Arthur_Jayson_Ilan_UA2.Dialogs.ViewModels
{
    public class EditUserViewModel : INotifyPropertyChanged
    {
        private readonly Model.User _user;

        // Indique si l'utilisateur à éditer est le SuperAdmin
        public bool IsEditingSuperAdmin => _user.RoleId == (int)UserRole.SuperAdmin;                
        public bool IsEditingClient => _user.RoleId == (int)UserRole.Client;        

        private readonly string _initialUsername;
        private readonly string _initialEmail;
        private readonly UserRole _initialRole;
        private readonly bool _initialIsActive;

        private HashSet<string> _modifiedProperties = new();

        public EditUserViewModel(Model.User user)
        {
            _user = user;

            _initialUsername = _user.Username;
            _initialEmail = _user.Email;
            _initialRole = (UserRole)_user.RoleId;
            _initialIsActive = (_user.IsActive == 1);

            Username = _initialUsername;
            Email = _initialEmail;
            Role = _initialRole;
            IsActive = _initialIsActive;

            AvailableRoles = GetAvailableRoles();

            IsUserEnabled = true;
            IsEmailEnabled = true;

            // Si l'utilisateur est SuperAdmin, désactiver le ComboBox des rôles et la CheckBox IsActive
            if (IsEditingSuperAdmin)
            {
                IsRoleEnabled = false;

                IsActive = true;
                IsActiveEnabled = false;

                IsUserEnabled = true;
                IsEmailEnabled = true;
            }
            else
            {
                IsUserEnabled = false;
                IsEmailEnabled = false;
                IsActiveEnabled = true;

                if (App.UserService.CurrentUser.IsSuperAdmin)
                {
                    IsRoleEnabled = true;
                }
                else
                {
                    IsActive = true;
                    IsRoleEnabled = false;

                    if (!IsEditingClient && App.UserService.CurrentUser.Username != _user.Username)
                    {
                        IsActiveEnabled = false;
                    }
                    if (!IsEditingClient && App.UserService.CurrentUser.Username == _user.Username )
                    {
                        IsActiveEnabled = false;
                        IsUserEnabled = true;
                        IsEmailEnabled = true;
                    }
                }
            }

            SaveCommand = new AsyncRelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); _modifiedProperties.Add(nameof(Username)); }
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); _modifiedProperties.Add(nameof(Email)); }
        }

        private UserRole _role;
        public UserRole Role
        {
            get => _role;
            set { _role = value; OnPropertyChanged(nameof(Role)); _modifiedProperties.Add(nameof(Role)); }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set { _isActive = value; OnPropertyChanged(nameof(IsActive)); _modifiedProperties.Add(nameof(IsActive)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private async Task Save(object? parameter)
        {
            try
            {

                // Mettre à jour uniquement les propriétés modifiées
                if (_modifiedProperties.Contains(nameof(Username)) && Username != _initialUsername)
                {
                    await App.userService.UpdateUsernameAsync(_user, Username);
                }

                if (_modifiedProperties.Contains(nameof(Email)) && Email != _initialEmail)
                {
                    await App.userService.UpdateEmailAsync(_user, Email);
                }

                if (_modifiedProperties.Contains(nameof(Role)) && Role != _initialRole)
                {
                    switch (Role)
                    {
                        case UserRole.SuperAdmin:
                            break;
                        case UserRole.Administrator:
                            await App.userService.PromoteToAdminAsync(App.userService.CurrentUser, _user);
                            break;
                        case UserRole.Librarian:
                            await App.userService.PromoteToLibrarianAsync(App.userService.CurrentUser, _user);
                            break;
                        case UserRole.Client:
                            await App.userService.DemoteToClientAsync(App.userService.CurrentUser, _user);
                            break;
                    }
                }

                if (_modifiedProperties.Contains(nameof(IsActive)) && IsActive != _initialIsActive)
                {
                    await App.userService.MakeNotActiveAsync(_user);
                }

                // Fermer la fenêtre avec un résultat de succès
                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sauvegarde : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Propriétés pour contrôler l'activation des contrôles dans le XAML
        private bool _isRoleEnabled;
        public bool IsRoleEnabled
        {
            get => _isRoleEnabled;
            set { _isRoleEnabled = value; OnPropertyChanged(nameof(IsRoleEnabled)); }
        }

        private bool _isActiveEnabled;
        public bool IsActiveEnabled
        {
            get => _isActiveEnabled;
            set { _isActiveEnabled = value; OnPropertyChanged(nameof(IsActiveEnabled)); }
        }

        private bool _isUserEnabled;
        public bool IsUserEnabled
        {
            get => _isUserEnabled;
            set { _isUserEnabled = value; OnPropertyChanged(nameof(IsUserEnabled)); }
        }

        private bool _isEmailEnabled;
        public bool IsEmailEnabled
        {
            get => _isEmailEnabled;
            set { _isEmailEnabled = value; OnPropertyChanged(nameof(IsEmailEnabled)); }
        }

        private bool CanSave(object? parameter)
        {
            // Implémenter la logique de validation avant de permettre la sauvegarde
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Email) && Regex.IsMatch(Email, @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,6}$");
        }

        private void Cancel(object? parameter)
        {
            // Fermer la fenêtre sans sauvegarder
            if (parameter is Window window)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        public ObservableCollection<UserRole> AvailableRoles { get; }

        private ObservableCollection<UserRole> GetAvailableRoles()
        {
            var roles = new ObservableCollection<UserRole>();
            
            if (IsEditingSuperAdmin)
            {
                roles.Add(UserRole.SuperAdmin);
            }
            else
            {
                foreach (var role in UserRoleValues.Roles)
                {
                    if (role != UserRole.SuperAdmin)
                    {
                        roles.Add(role);
                    }
                }
            }
            return roles;
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
