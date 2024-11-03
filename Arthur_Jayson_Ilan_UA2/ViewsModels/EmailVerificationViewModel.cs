using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Models;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Services;
using Arthur_Jayson_Ilan_UA2.Views;
using System.Runtime.InteropServices;
using System.Security;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels
{
    public class EmailVerificationViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;

        // Flag pour éviter les boucles infinies
        private bool _isUpdatingSuperAdminPassword = false;

        // Propriétés pour les champs de saisie
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

        private bool _isEmailEnabled = true;
        public bool IsEmailEnabled
        {
            get => _isEmailEnabled;
            set
            {
                if (_isEmailEnabled != value)
                {
                    _isEmailEnabled = value;
                    OnPropertyChanged(nameof(IsEmailEnabled));
                }
            }
        }

        private string _superAdminUsername = string.Empty;
        public string SuperAdminUsername
        {
            get => _superAdminUsername;
            set
            {
                if (_superAdminUsername != value)
                {
                    _superAdminUsername = value;
                    OnPropertyChanged(nameof(SuperAdminUsername));
                    SuperAdminUsernameError = string.Empty;
                }
            }
        }

        private SecureString _superAdminPassword = new SecureString();
        public SecureString SuperAdminPassword
        {
            get => _superAdminPassword;
            set
            {
                if (_superAdminPassword != value)
                {
                    _superAdminPassword = value;
                    OnPropertyChanged(nameof(SuperAdminPassword));
                    SuperAdminPasswordError = string.Empty;

                    if (!_isUpdatingSuperAdminPassword)
                    {
                        try
                        {
                            _isUpdatingSuperAdminPassword = true;
                            SuperAdminPasswordUnsecure = ConvertToUnsecureString(_superAdminPassword);
                        }
                        finally
                        {
                            _isUpdatingSuperAdminPassword = false;
                        }
                    }
                }
            }
        }

        // Propriété pour afficher les mots de passe en clair
        private string _superAdminPasswordUnsecure = string.Empty;
        public string SuperAdminPasswordUnsecure
        {
            get => _superAdminPasswordUnsecure;
            set
            {
                if (_superAdminPasswordUnsecure != value)
                {
                    _superAdminPasswordUnsecure = value;
                    OnPropertyChanged(nameof(SuperAdminPasswordUnsecure));

                    if (!_isUpdatingSuperAdminPassword)
                    {
                        try
                        {
                            _isUpdatingSuperAdminPassword = true;
                            UpdateSecurePassword(value, ref _superAdminPassword);
                        }
                        finally
                        {
                            _isUpdatingSuperAdminPassword = false;
                        }
                    }
                }
            }
        }

        // Propriétés pour les messages d'erreur
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

        private string _superAdminUsernameError = string.Empty;
        public string SuperAdminUsernameError
        {
            get => _superAdminUsernameError;
            set
            {
                if (_superAdminUsernameError != value)
                {
                    _superAdminUsernameError = value;
                    OnPropertyChanged(nameof(SuperAdminUsernameError));
                }
            }
        }

        private string _superAdminPasswordError = string.Empty;
        public string SuperAdminPasswordError
        {
            get => _superAdminPasswordError;
            set
            {
                if (_superAdminPasswordError != value)
                {
                    _superAdminPasswordError = value;
                    OnPropertyChanged(nameof(SuperAdminPasswordError));
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

        // Propriété pour gérer la visibilité des champs de super admin
        private bool _isSuperAdminAuthVisible;
        public bool IsSuperAdminAuthVisible
        {
            get => _isSuperAdminAuthVisible;
            set
            {
                if (_isSuperAdminAuthVisible != value)
                {
                    _isSuperAdminAuthVisible = value;
                    OnPropertyChanged(nameof(IsSuperAdminAuthVisible));
                    // Désactiver les champs d'email lorsque les champs du super admin sont visibles
                    IsEmailEnabled = !_isSuperAdminAuthVisible;
                }
            }
        }

        // Propriétés pour la gestion de la visibilité des mots de passe
        private bool _isSuperAdminPasswordVisible;
        public bool IsSuperAdminPasswordVisible
        {
            get => _isSuperAdminPasswordVisible;
            set
            {
                if (_isSuperAdminPasswordVisible != value)
                {
                    _isSuperAdminPasswordVisible = value;
                    OnPropertyChanged(nameof(IsSuperAdminPasswordVisible));
                }
            }
        }

        // Commandes
        public ICommand VerifyEmailCommand { get; }
        public ICommand AuthenticateSuperAdminCommand { get; }
        public ICommand ReturnCommand { get; }
        public ICommand ToggleSuperAdminPasswordVisibilityCommand { get; }

        // Propriété pour stocker le type de reset (password ou username)
        private string _resetType = "password"; // Par défaut

        public EmailVerificationViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Initialisation des commandes
            VerifyEmailCommand = new AsyncRelayCommand(ExecuteVerifyEmailAsync);
            AuthenticateSuperAdminCommand = new AsyncRelayCommand(ExecuteAuthenticateSuperAdminAsync);
            ReturnCommand = new RelayCommand(ExecuteReturn);
            ToggleSuperAdminPasswordVisibilityCommand = new RelayCommand(ToggleSuperAdminPasswordVisibility);

            IsSuperAdminPasswordVisible = false;
            IsSuperAdminAuthVisible = false;
        }

        // Méthode pour définir le type de reset
        public void SetResetType(string resetType)
        {
            _resetType = resetType;
        }

        // Méthode pour vérifier l'email
        private async Task ExecuteVerifyEmailAsync(object? parameter)
        {
            // Réinitialiser les messages
            ResetErrorMessage = string.Empty;
            ResetSuccessMessage = string.Empty;

            bool isValid = ValidateEmail();

            if (isValid)
            {
                var user = App.UserService.FindUserByEmail(Email.Trim());

                if (user != null)
                {
                    if (user.IsSuperAdmin)
                    {
                        ShowSuperAdminAuthFields();
                    }
                    else
                    {
                        await ProceedToResetAsync(user);
                    }
                }
                else
                {
                    ShowEmailError("Cette adresse e-mail n'est pas enregistrée.");
                }
            }
        }

        // Méthode pour authentifier le super admin
        private async Task ExecuteAuthenticateSuperAdminAsync(object? parameter)
        {
            var superAdmin = App.UserService.FindUserByEmail(Email.Trim());

            // Conversion de SecureString en string de manière sécurisée
            string? superAdminPassword = ConvertToUnsecureString(SuperAdminPassword);

            if (superAdmin != null && superAdmin.VerifyPassword(superAdminPassword) && superAdmin.Username == SuperAdminUsername.Trim())
            {
                await ProceedToResetAsync(superAdmin);
            }
            else
            {
                ShowSuperAdminAuthError("Nom d'utilisateur ou mot de passe incorrect.");
            }

            // Effacer la chaîne en mémoire
            superAdminPassword = null;
        }

        // Méthode pour revenir à la vue précédente
        private void ExecuteReturn(object? parameter)
        {
            _navigationService.NavigateTo(new LoginView());
        }

        // Méthode pour valider l'email
        private bool ValidateEmail()
        {
            bool isValid = true;
            string emailPattern = @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,6}$";
            string emailInput = Email.Trim();
            EmailError = string.Empty;

            if (string.IsNullOrEmpty(emailInput) || !Regex.IsMatch(emailInput, emailPattern))
            {
                ShowEmailError("Veuillez entrer une adresse e-mail valide.");
                isValid = false;
            }

            return isValid;
        }

        // Méthode pour afficher une erreur d'email
        private void ShowEmailError(string message)
        {
            EmailError = message;
        }

        // Méthode pour afficher une erreur d'authentification du super admin
        private void ShowSuperAdminAuthError(string message)
        {
            SuperAdminPasswordError = message;
        }

        // Méthode pour afficher les champs d'authentification du super admin
        private async void ShowSuperAdminAuthFields()
        {
            ResetSuccessMessage = "Veuillez entrer les informations actuelles de connexion pour le super admin.";

            // Simuler un délai
            await Task.Delay(2000);

            IsSuperAdminAuthVisible = true;
        }

        // Méthode pour procéder au reset (redirection)
        private async Task ProceedToResetAsync(User user)
        {
            ResetErrorMessage = string.Empty;
            ResetSuccessMessage = "Adresse e-mail vérifiée avec succès.";

            // Simuler un délai avant de naviguer
            await Task.Delay(1000);

            // Naviguer vers la vue de reset appropriée
            if (_resetType == "password")
            {
                _navigationService.NavigateTo(new ResetPasswordView(user));
            }
            else if (_resetType == "username")
            {
                _navigationService.NavigateTo(new ResetUsernameView(user));
            }
        }

        // Méthode pour basculer la visibilité du mot de passe super admin
        private void ToggleSuperAdminPasswordVisibility(object? parameter)
        {
            IsSuperAdminPasswordVisible = !IsSuperAdminPasswordVisible;
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
