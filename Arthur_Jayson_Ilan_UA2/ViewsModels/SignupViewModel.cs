using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Services;
using Arthur_Jayson_Ilan_UA2.Views;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels
{
    public class SignupViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;

        // Propriétés pour les champs de saisie
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

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                    EmailError = string.Empty;
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

        private bool _isConfirmPasswordVisible;
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

        private string _emailError = string.Empty;
        public string EmailError
        {
            get => _emailError;
            set
            {
                if (_emailError != value)
                {
                    _emailError = value;
                    OnPropertyChanged(nameof(EmailError));
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

        private string _signupErrorMessage = string.Empty;
        public string SignupErrorMessage
        {
            get => _signupErrorMessage;
            set
            {
                if (_signupErrorMessage != value)
                {
                    _signupErrorMessage = value;
                    OnPropertyChanged(nameof(SignupErrorMessage));
                }
            }
        }

        private string _signupSuccessMessage = string.Empty;
        public string SignupSuccessMessage
        {
            get => _signupSuccessMessage;
            set
            {
                if (_signupSuccessMessage != value)
                {
                    _signupSuccessMessage = value;
                    OnPropertyChanged(nameof(SignupSuccessMessage));
                }
            }
        }

        // Commandes
        public ICommand RegisterCommand { get; }
        public ICommand AlreadyHaveAccountCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand ToggleConfirmPasswordVisibilityCommand { get; }

        public SignupViewModel(INavigationService navigationService)
        {

            _navigationService = navigationService;

            RegisterCommand = new RelayCommand(ExecuteRegister);
            AlreadyHaveAccountCommand = new RelayCommand(ExecuteAlreadyHaveAccount);
            TogglePasswordVisibilityCommand = new RelayCommand(TogglePasswordVisibility);
            ToggleConfirmPasswordVisibilityCommand = new RelayCommand(ToggleConfirmPasswordVisibility);
            IsPasswordVisible = false;
            IsConfirmPasswordVisible = false;
        }

        private async void ExecuteRegister(object? parameter)
        {
            // Réinitialiser les messages
            SignupErrorMessage = string.Empty;
            SignupSuccessMessage = string.Empty;

            bool hasError = false;

            // Validation des champs
            if (string.IsNullOrWhiteSpace(Username))
            {
                UsernameError = "Veuillez entrer un nom d'utilisateur";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Veuillez entrer une adresse e-mail.";
                hasError = true;
            }
            else if (!IsValidEmail(Email))
            {
                EmailError = "Veuillez entrer une adresse e-mail valide.";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Veuillez entrer un mot de passe.";
                hasError = true;
            }
            else if (Password.Length < 12)
            {
                PasswordError = "Le mot de passe doit comporter au moins 12 caractères.";
                hasError = true;
            }
            else if (!(Password.Any(char.IsUpper) && Password.Any(char.IsLower) && Password.Any(char.IsDigit)))
            {
                PasswordError = "Le mot de passe doit contenir au moins une majuscule, une minuscule, et un chiffre.";
                hasError = true;
            }
            else
            {
                if (Password != ConfirmPassword)
                {
                    ConfirmPasswordError = "Les mots de passe ne correspondent pas";
                    hasError = true;
                }
            }

            if (!hasError)
            {
                try
                {
                    // Appeler le service d'inscription
                    App.UserService.RegisterUser(Username, Password, Email);

                    // Attendre un court instant
                    await Task.Delay(200);

                    SignupSuccessMessage = "Inscription réussie !";

                    // Attendre avant de naviguer
                    await Task.Delay(1000);

                    NavigateToLoginView();
                }
                catch (Exception ex)
                {
                    SignupErrorMessage = ex.Message;
                }
            }
        }

        private void ExecuteAlreadyHaveAccount(object? parameter)
        {
            NavigateToLoginView();
        }

        private void TogglePasswordVisibility(object? parameter)
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        private void ToggleConfirmPasswordVisibility(object? parameter)
        {
            IsConfirmPasswordVisible = !IsConfirmPasswordVisible;
        }

        private void NavigateToLoginView()
        {
            _navigationService.NavigateTo(new LoginView());
        }

        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,6}$";
            return Regex.IsMatch(email, emailPattern);
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
