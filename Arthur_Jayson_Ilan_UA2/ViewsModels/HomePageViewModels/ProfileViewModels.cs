using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Arthur_Jayson_Ilan_UA2.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private User _currentUser = new();

        public User CurrentUser
        {
            get => _currentUser;
            set { _currentUser = value; OnPropertyChanged(); }
        }

        public ProfileViewModel()
        {
            CurrentUser = new User
            {
                Username = "Arthur",
                Email = "arthur@example.com",
                ProfileImagePath = "./Assets/Images/superAdminAccount.png"
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfileImagePath { get; set; } = string.Empty;
    }
}
