using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Models;
using Arthur_Jayson_Ilan_UA2.Services;
using Arthur_Jayson_Ilan_UA2.Views;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.LoginPageViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        //private readonly INavigationService _navigationService;

        // Flag pour éviter les boucles infinies
        private bool _isUpdatingPassword = false;

        public LoginViewModel() // INavigationService navigationService
        {
            //_navigationService = navigationService;

            ConnectCommand = new AsyncRelayCommand(ExecuteConnectAsync);
            TogglePasswordVisibilityCommand = new RelayCommand(TogglePasswordVisibility);
            ResetDataCommand = new RelayCommand(ExecuteResetData);
            NotHaveAccountCommand = new RelayCommand(ExecuteNotHaveAccount);
            IsPasswordVisible = false;
        }

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                    UsernameError = string.Empty;
                }
            }
        }

        private SecureString _password = new SecureString();
        public SecureString Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password.Dispose(); // Libérer les ressources de l'ancien SecureString
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

        // Propriété pour afficher les mots de passe en clair
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
                            UpdateSecurePassword(value);
                        }
                        finally
                        {
                            _isUpdatingPassword = false;
                        }
                    }
                }
            }
        }

        private bool _isPasswordVisible;
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

        private string _usernameError = string.Empty;
        public string UsernameError
        {
            get => _usernameError;
            set
            {
                if (_usernameError != value)
                {
                    _usernameError = value;
                    OnPropertyChanged(nameof(UsernameError));
                }
            }
        }

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

        private string _loginErrorMessage = string.Empty;
        public string LoginErrorMessage
        {
            get => _loginErrorMessage;
            set
            {
                if (_loginErrorMessage != value)
                {
                    _loginErrorMessage = value;
                    OnPropertyChanged(nameof(LoginErrorMessage));
                }
            }
        }

        public ICommand ConnectCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }

        public ICommand ResetDataCommand { get; }
        public ICommand NotHaveAccountCommand { get; }

        private async Task ExecuteConnectAsync(object? parameter)
        {
            LoginErrorMessage = string.Empty;

            bool hasError = false;

            // Conversion de SecureString en string de manière sécurisée
            string? password = ConvertToUnsecureString(Password);

            if (string.IsNullOrWhiteSpace(Username))
            {
                UsernameError = "Veuillez entrer un nom d'utilisateur.";
                hasError = true;
            }
            else
            {
                UsernameError = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                PasswordError = "Veuillez entrer un mot de passe.";
                hasError = true;
            }
            else
            {
                PasswordError = string.Empty;
            }

            if (!hasError)
            {
                try
                {
                    var user = await App.UserService.AuthenticateAsync(Username, password);

                    // Effacer la chaîne en mémoire
                    password = null;

                    if (user != null)
                    {
                        // Connexion réussie
                        if (user.IsActive)
                            NavigationService.Instance.OpenWindow<HomePage>(user);
                        else
                            LoginErrorMessage = "Accès refusé.";
                    }
                    else
                    {
                        LoginErrorMessage = "Nom d'utilisateur ou mot de passe incorrect.";
                    }
                }
                catch (Exception ex)
                {
                    LoginErrorMessage = $"Une erreur s'est produite : {ex.Message}";
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void TogglePasswordVisibility(object? parameter)
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        private void ExecuteNotHaveAccount(object? parameter)
        {
            //_navigationService.NavigateTo(new SignupView());
            NavigationService.Instance.NavigateTo(new SignupView());
        }

        private void ExecuteResetData(object? parameter)
        {
            if (parameter is string resetType)
            {
                // Implémenter la navigation vers la vue de récupération des identifiants
                NavigationService.Instance.NavigateTo(new EmailVerificationView(resetType));
            }
        }

        // Méthode pour convertir SecureString en string de manière sécurisée
        private static string ConvertToUnsecureString(SecureString secureString)
        {
            if (secureString == null)
                return string.Empty;

            nint unmanagedString = nint.Zero;
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
        private void UpdateSecurePassword(string unsecurePassword, bool isConfirm = false)
        {
            var newSecurePassword = new SecureString();

            if (!string.IsNullOrEmpty(unsecurePassword))
            {
                foreach (char c in unsecurePassword)
                {
                    newSecurePassword.AppendChar(c);
                }
            }
            newSecurePassword.MakeReadOnly();

            if (!isConfirm)
            {
                Password = newSecurePassword;
            }
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
