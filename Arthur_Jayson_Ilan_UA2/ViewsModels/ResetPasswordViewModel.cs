using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
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

        // Flags pour éviter les boucles infinies
        private bool _isUpdatingPassword = false;
        private bool _isUpdatingConfirmPassword = false;

        // Propriétés liées aux champs de mot de passe
        private SecureString _password = new SecureString();
        public SecureString Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                    PasswordError = string.Empty;

                    if (!_isUpdatingPassword)
                    {
                        try
                        {
                            _isUpdatingPassword = true;
                            PasswordUnsecure = ConvertToUnsecureString(_password);
                        }
                        finally
                        {
                            _isUpdatingPassword = false;
                        }
                    }
                }
            }
        }

        private SecureString _confirmPassword = new SecureString();
        public SecureString ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    OnPropertyChanged(nameof(ConfirmPassword));
                    ConfirmPasswordError = string.Empty;

                    if (!_isUpdatingConfirmPassword)
                    {
                        try
                        {
                            _isUpdatingConfirmPassword = true;
                            ConfirmPasswordUnsecure = ConvertToUnsecureString(_confirmPassword);
                        }
                        finally
                        {
                            _isUpdatingConfirmPassword = false;
                        }
                    }
                }
            }
        }

        // Propriétés pour afficher les mots de passe en clair
        private string _passwordUnsecure = string.Empty;
        public string PasswordUnsecure
        {
            get => _passwordUnsecure;
            set
            {
                if (_passwordUnsecure != value)
                {
                    _passwordUnsecure = value;
                    OnPropertyChanged(nameof(PasswordUnsecure));

                    if (!_isUpdatingPassword)
                    {
                        try
                        {
                            _isUpdatingPassword = true;
                            UpdateSecurePassword(value, ref _password);
                        }
                        finally
                        {
                            _isUpdatingPassword = false;
                        }
                    }
                }
            }
        }

        private string _confirmPasswordUnsecure = string.Empty;
        public string ConfirmPasswordUnsecure
        {
            get => _confirmPasswordUnsecure;
            set
            {
                if (_confirmPasswordUnsecure != value)
                {
                    _confirmPasswordUnsecure = value;
                    OnPropertyChanged(nameof(ConfirmPasswordUnsecure));

                    if (!_isUpdatingConfirmPassword)
                    {
                        try
                        {
                            _isUpdatingConfirmPassword = true;
                            UpdateSecurePassword(value, ref _confirmPassword);
                        }
                        finally
                        {
                            _isUpdatingConfirmPassword = false;
                        }
                    }
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
                    // Conversion de SecureString en string de manière sécurisée
                    string? password = ConvertToUnsecureString(Password);

                    // Mise à jour du mot de passe via le UserService
                    App.UserService.UpdatePassword(_currentUser, password);

                    // Effacer la chaîne en mémoire
                    password = null;

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

            string? password = ConvertToUnsecureString(Password);
            string? confirmPassword = ConvertToUnsecureString(ConfirmPassword);

            if (password.Length < 12)
            {
                PasswordError = "Le mot de passe doit contenir au moins 12 caractères.";
                isValid = false;
            }
            else if (!password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit))
            {
                PasswordError = "Le mot de passe doit contenir au moins une majuscule, une minuscule et un chiffre.";
                isValid = false;
            }
            else
            {
                if (password != confirmPassword)
                {
                    ConfirmPasswordError = "Les mots de passe ne correspondent pas.";
                    isValid = false;
                }
            }

            // Effacer les chaînes en mémoire
            password = null;
            confirmPassword = null;

            return isValid;
        }

        // Méthode pour convertir SecureString en string de manière sécurisée
        private static string ConvertToUnsecureString(SecureString secureString)
        {
            if (secureString == null)
                return string.Empty;

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString) ?? string.Empty;
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        // Méthode pour mettre à jour le SecureString à partir d'un string
        private static void UpdateSecurePassword(string unsecurePassword, ref SecureString securePassword)
        {
            // Effacer le SecureString existant
            securePassword.Clear();

            if (!string.IsNullOrEmpty(unsecurePassword))
            {
                foreach (char c in unsecurePassword)
                {
                    securePassword.AppendChar(c);
                }
                securePassword.MakeReadOnly();
            }
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
