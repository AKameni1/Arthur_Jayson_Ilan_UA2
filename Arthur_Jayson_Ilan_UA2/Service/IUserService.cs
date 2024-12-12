using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Model;
using Arthur_Jayson_Ilan_UA2.Services;

namespace Arthur_Jayson_Ilan_UA2.Service
{
    public interface IUserService
    {
        Task RegisterUserAsync(string username, string password, string email);
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
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetUsersByRoleAsync(UserRole role);
        Task<int> CountUsersByRoleAsync(UserRole role);
        Task MakeNotActiveAsync(User user);
        void Logout();
    }
}
