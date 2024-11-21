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
        Task<User> RegisterSuperAdminAsync(string username, string password, string email);
        Task<User> RegisterUserAsync(string username, string password, string email);
        Task<User?> AuthenticateAsync(string username, string password);
        Task<User?> FindUserByIdAsync(int userId);
        Task<User?> FindUserByUsernameAsync(string username);
        Task<User?> FindUserByEmailAsync(string email);
        Task UpdatePasswordAsync(User user, string newPassword);
        Task UpdateEmailAsync(User user, string newEmail);
        Task UpdateUsernameAsync(User user, string newUsername);
        Task PromoteToAdminAsync(User currentUser, User targetUser);
        Task PromoteToLibrarianAsync(User currentUser, User targetUser);
        Task DemoteToClientAsync(User currentUser, User targetUser);
        Task DeleteUserAsync(User currentUser, User userToDelete);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
        Task<int> CountUsersByRoleAsync(UserRole role);
        Task ToggleUserActiveStatusAsync(User user);
        Task LogoutAsync();

        User CurrentUser { get; }

        event EventHandler<string>? MessageSent;
    }
}
