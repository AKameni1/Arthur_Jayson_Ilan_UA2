using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Models;
using Arthur_Jayson_Ilan_UA2.Services;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Views;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.LoginPageViewModels
{
    public class ResetUsernameViewModel : INotifyPropertyChanged
    {

        // Propriétés liées au champ de nom d'utilisateur
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

        // Propriétés pour les messages d'erreur et de succès
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

        // Services et dépendances
        //private readonly INavigationService _navigationService;
        private readonly User _currentUser;

        public ResetUsernameViewModel(User user) // INavigationService navigationService, 
        {
            //_navigationService = navigationService;
            _currentUser = user;

            ConfirmCommand = new AsyncRelayCommand(ExecuteConfirmAsync);
            ReturnCommand = new RelayCommand(ExecuteReturn);
        }

        // Méthodes des commandes
        private async Task ExecuteConfirmAsync(object? parameter)
        {
            ResetErrorMessage = string.Empty;
            ResetSuccessMessage = string.Empty;

            if (await ValidateUsernameAsync())
            {
                try
                {
                    // Mise à jour du nom d'utilisateur via le UserService
                    await App.UserService.UpdateUsernameAsync(_currentUser, Username.Trim());

                    // Affichage du message de succès
                    ResetSuccessMessage = "Nom d'utilisateur mis à jour avec succès !";
                    await Task.Delay(500);

                    // Navigation vers la vue de connexion
                    NavigationService.Instance.NavigateTo(new LoginView());
                }
                catch (Exception ex)
                {
                    // Affichage du message d'erreur
                    ResetErrorMessage = $"Une erreur s'est produite : {ex.Message}";
                }
            }
        }

        private void ExecuteReturn(object? parameter)
        {
            // Navigation vers la vue de connexion
            NavigationService.Instance.NavigateTo(new LoginView());
        }

        // Méthode de validation du nom d'utilisateur
        private async Task<bool> ValidateUsernameAsync()
        {
            UsernameError = string.Empty;
            ResetErrorMessage = string.Empty;

            bool isValid = true;

            string newUsername = Username.Trim();

            if (string.IsNullOrWhiteSpace(newUsername))
            {
                UsernameError = "Le nom d'utilisateur ne peut pas être vide.";
                isValid = false;
            }
            else
            {
                try
                {
                    bool exists = await App.UserService.FindUserByUsernameAsync(newUsername) != null;
                    if (exists)
                    {
                        UsernameError = "Ce nom d'utilisateur est déjà pris.";
                        isValid = false;
                    }
                }
                catch (Exception ex)
                {
                    // Gérer les exceptions, par exemple en enregistrant ou en affichant un message d'erreur
                    ResetErrorMessage = $"Erreur lors de la validation du nom d'utilisateur : {ex.Message}";
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
