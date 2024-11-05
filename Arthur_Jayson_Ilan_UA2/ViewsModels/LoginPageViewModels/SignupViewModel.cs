using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Services;
using Arthur_Jayson_Ilan_UA2.Views;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.LoginPageViewModels
{
    public class SignupViewModel : INotifyPropertyChanged
    {
        //private readonly INavigationService _navigationService;

        // Flags pour éviter les boucles infinies
        private bool _isUpdatingPassword = false;
        private bool _isUpdatingConfirmPassword = false;

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
                            UpdateSecurePassword(value, isConfirm: true);
                        }
                        finally
                        {
                            _isUpdatingConfirmPassword = false;
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

        public SignupViewModel() // INavigationService navigationService
        {

            //_navigationService = navigationService;

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

            string? password = ConvertToUnsecureString(Password);
            string? confirmPassword = ConvertToUnsecureString(ConfirmPassword);

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

            if (string.IsNullOrWhiteSpace(password))
            {
                PasswordError = "Veuillez entrer un mot de passe.";
                hasError = true;
            }
            else if (password.Length < 12)
            {
                PasswordError = "Le mot de passe doit comporter au moins 12 caractères.";
                hasError = true;
            }
            else if (!(password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit)))
            {
                PasswordError = "Le mot de passe doit contenir au moins une majuscule, une minuscule, et un chiffre.";
                hasError = true;
            }
            else
            {
                if (password != confirmPassword)
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
                    App.UserService.RegisterUser(Username, password, Email);

                    // Effacer les chaînes en mémoire
                    password = null;
                    confirmPassword = null;

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
            NavigationService.Instance.NavigateTo(new LoginView());
        }

        private static bool IsValidEmail(string email)
        {
            string emailPattern = @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,6}$";
            return Regex.IsMatch(email, emailPattern);
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

            if (isConfirm)
            {
                ConfirmPassword = newSecurePassword;
            }
            else
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
