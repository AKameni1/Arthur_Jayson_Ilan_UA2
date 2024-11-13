using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Arthur_Jayson_Ilan_UA2.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private User _currentUser;

        public User CurrentUser
        {
            get => _currentUser;
            set { _currentUser = value; OnPropertyChanged(); }
        }

        public ProfileViewModel()
        {
            // Simule un utilisateur connecté. Remplacez cette logique par une requête à votre base de données.
            CurrentUser = new User
            {
                Username = "Arthur",
                Email = "arthur@example.com",
                ProfileImagePath = "/Images/default-profile.png" // Chemin de l'image par défaut
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfileImagePath { get; set; }
    }
}
