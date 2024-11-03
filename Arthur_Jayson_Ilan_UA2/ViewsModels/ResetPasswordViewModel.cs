using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Models;
using Arthur_Jayson_Ilan_UA2.Services;
using Arthur_Jayson_Ilan_UA2.Views;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels
{
    public class ResetPasswordViewModel : INotifyPropertyChanged
    {

        // Propriétés liées aux champs de mot de passe
        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                    PasswordError = string.Empty;
                }
            }
        }

        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    OnPropertyChanged(nameof(ConfirmPassword));
                    ConfirmPasswordError = string.Empty;
                }
            }
        }

        // Propriétés pour la visibilité des mots de passe
        private bool _isPasswordVisible = false;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                if (_isPasswordVisible != value)
                {
                    _isPasswordVisible = value;
                    OnPropertyChanged(nameof(IsPasswordVisible));
                }
            }
        }

        private bool _isConfirmPasswordVisible = false;
        public bool IsConfirmPasswordVisible
        {
            get => _isConfirmPasswordVisible;
            set
            {
                if (_isConfirmPasswordVisible != value)
                {
                    _isConfirmPasswordVisible = value;
                    OnPropertyChanged(nameof(IsConfirmPasswordVisible));
                }
            }
        }

        // Propriétés pour les messages d'erreur et de succès
        private string _passwordError = string.Empty;
        public string PasswordError
        {
            get => _passwordError;
            set
            {
                if (_passwordError != value)
                {
                    _passwordError = value;
                    OnPropertyChanged(nameof(PasswordError));
                }
            }
        }

        private string _confirmPasswordError = string.Empty;
        public string ConfirmPasswordError
        {
            get => _confirmPasswordError;
            set
            {
                if (_confirmPasswordError != value)
                {
                    _confirmPasswordError = value;
                    OnPropertyChanged(nameof(ConfirmPasswordError));
                }
            }
        }

        private string _resetErrorMessage = string.Empty;
        public string ResetErrorMessage
        {
            get => _resetErrorMessage;
            set
            {
                if (_resetErrorMessage != value)
                {
                    _resetErrorMessage = value;
                    OnPropertyChanged(nameof(ResetErrorMessage));
                }
            }
        }

        private string _resetSuccessMessage = string.Empty;
        public string ResetSuccessMessage
        {
            get => _resetSuccessMessage;
            set
            {
                if (_resetSuccessMessage != value)
                {
                    _resetSuccessMessage = value;
                    OnPropertyChanged(nameof(ResetSuccessMessage));
                }
            }
        }

        // Commandes
        public ICommand ConfirmCommand { get; }
        public ICommand ReturnCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand ToggleConfirmPasswordVisibilityCommand { get; }

        // Services et dépendances
        private readonly INavigationService _navigationService;
        private readonly User _currentUser;

        public ResetPasswordViewModel(INavigationService navigationService, User user)
        {
            _navigationService = navigationService;            
            _currentUser = user;

            ConfirmCommand = new AsyncRelayCommand(ExecuteConfirmAsync);
            ReturnCommand = new RelayCommand(ExecuteReturn);
            TogglePasswordVisibilityCommand = new RelayCommand(ExecuteTogglePasswordVisibility);
            ToggleConfirmPasswordVisibilityCommand = new RelayCommand(ExecuteToggleConfirmPasswordVisibility);
        }

        // Méthodes des commandes
        private async Task ExecuteConfirmAsync(object? parameter)
        {
            ResetErrorMessage = string.Empty;
            ResetSuccessMessage = string.Empty;

            if (ValidatePassword())
            {
                try
                {
                    // Mise à jour du mot de passe via le UserService
                    App.UserService.UpdatePassword(_currentUser, Password);

                    // Affichage du message de succès
                    ResetSuccessMessage = "Mot de passe mis à jour avec succès.";
                    await Task.Delay(1000);

                    // Navigation vers la vue de connexion
                    _navigationService.NavigateTo(new LoginView());
                }
                catch (Exception ex)
                {
                    // Affichage du message d'erreur
                    ResetErrorMessage = $"Une erreur inattendue s'est produite : {ex.Message}";
                }
            }
        }

        private void ExecuteReturn(object? parameter)
        {
            // Navigation vers la vue de connexion
            _navigationService.NavigateTo(new LoginView());
        }

        private void ExecuteTogglePasswordVisibility(object? parameter)
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        private void ExecuteToggleConfirmPasswordVisibility(object? parameter)
        {
            IsConfirmPasswordVisible = !IsConfirmPasswordVisible;
        }

        // Méthode de validation des mots de passe
        private bool ValidatePassword()
        {
            PasswordError = string.Empty;
            ConfirmPasswordError = string.Empty;

            bool isValid = true;

            if (Password.Length < 12)
            {
                PasswordError = "Le mot de passe doit contenir au moins 12 caractères.";
                isValid = false;
            }
            else if (!Password.Any(char.IsUpper) || !Password.Any(char.IsLower) || !Password.Any(char.IsDigit))
            {
                PasswordError = "Le mot de passe doit contenir au moins une majuscule, une minuscule et un chiffre.";
                isValid = false;
            }
            else
            {
                if (Password != ConfirmPassword)
                {
                    ConfirmPasswordError = "Les mots de passe ne correspondent pas.";
                    isValid = false;
                }
            }

            

            return isValid;
        }


        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
