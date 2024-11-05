using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Models;

namespace Arthur_Jayson_Ilan_UA2.Services
{
    public interface IUserService
    {
        void RegisterUser(string username, string password, string email);
        User? Authenticate(string username, string password);
        User? FindUserById(int userId);
        User? FindUserByUsername(string username);
        User? FindUserByEmail(string email);
        void UpdatePassword(User user, string newPassword);
        void UpdateEmail(User user, string newEmail);
        void UpdateUsername(User user, string newUsername);
        void PromoteToAdmin(User currentUser, User targetUser);
        void PromoteToLibrarian(User currentUser, User targetUser);
        void DemoteToClient(User currentUser, User targetUser);
        void DeleteUser(User currentUser, User userToDelete);
        void AddUser();
        ObservableCollection<User> GetAllUsers();
        ObservableCollection<User> GetUsersByRole(UserRole role);
        int CountUsersByRole(UserRole role);
        void MakeNotActive(User user);
    }
}
