using System.ComponentModel;
using System.Runtime.CompilerServices;
using Arthur_Jayson_Ilan_UA2.ViewsModels;

namespace Arthur_Jayson_Ilan_UA2.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private User _currentUser = new();

        public User CurrentUser
        {
            get => _currentUser;
            set { _currentUser = value; OnPropertyChanged(nameof(CurrentUser)); }
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
    }

    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfileImagePath { get; set; } = string.Empty;
    }
}
