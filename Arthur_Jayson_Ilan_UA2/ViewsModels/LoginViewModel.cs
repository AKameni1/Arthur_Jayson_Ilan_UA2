using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Models;
using Arthur_Jayson_Ilan_UA2.Services;
using Arthur_Jayson_Ilan_UA2.Views;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;

        public LoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            ConnectCommand = new RelayCommand(ExecuteConnect);
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

        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                if (_isPasswordVisible != value)
                {
                    _isPasswordVisible = value;
                    OnPropertyChanged( nameof(IsPasswordVisible));
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

        private string _passwordError = "";
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

        private void ExecuteConnect(object? parameter)
        {
            LoginErrorMessage = string.Empty;

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(Username))
            {
                UsernameError = "Veuillez entrer un nom d'utilisateur.";
                hasError = true;
            }
            else
            {
                UsernameError = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Veuillez entrer un mot de passe.";
                hasError= true;
            }
            else
            {
                PasswordError = string.Empty;
            }

            if (!hasError)
            {
                var user = App.UserService?.Authenticate(Username, Password);

                if (user != null)
                {
                    // Connexion réussie
                    if (user.IsSuperAdmin)
                    {
                        MessageBox.Show("Bienvenue, super administrateur!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Connexion réussie!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    // Naviguer vers la vue appropriée
                    // TODO: Implémenter la navigation
                }
                else
                {
                    LoginErrorMessage = "Nom d'utilisateur ou mot de passse incorrect.";
                }
            }
        }

        private void TogglePasswordVisibility(object? parameter)
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        private void ExecuteNotHaveAccount(object? parameter)
        {
            _navigationService.NavigateTo(new SignupView());
        }

        private void ExecuteResetData(object? parameter)
        {
            if (parameter is string resetType)
            {
                // Implémenter la navigation vers la vue de récupération des identifiants
                _navigationService.NavigateTo(new EmailVerificationView(resetType));
            }
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
