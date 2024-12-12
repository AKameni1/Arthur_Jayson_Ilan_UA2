using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Model;
using Arthur_Jayson_Ilan_UA2.Services;
using Microsoft.EntityFrameworkCore;

namespace Arthur_Jayson_Ilan_UA2.Service
{
    public class UserService : IUserService
    {
        public event EventHandler<string>? MessageSent;
        public User CurrentUser { get; private set; } = new User();
        protected void OnMessageSent(string message)
        {
            MessageSent?.Invoke(this, message);
        }


        private readonly LibraryContext _context = LibraryContextManager.Instance;

        /// <summary>
        /// Authentifie un utilisateur en vérifiant le nom d'utilisateur et le mot de passe.
        /// </summary>
        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>  u.Username == username);
            if (user != null && user.VerifyPassword(password))
            {
                CurrentUser = user;
                OnMessageSent($"Utilisateur '{username}' authentifié avec succès.");
                return user;
            }

            OnMessageSent($"Échec de l'authentification. Nom d'utilisateur ou mot de passe incorrect.");
            return null;
        }

        /// <summary>
        /// Compte le nombre d'utilisateurs par rôle.
        /// </summary>
        public async Task<int> CountUsersByRoleAsync(UserRole role)
        {
            return await _context.Users.CountAsync(u => u.RoleId == (int)role);
        }

        /// <summary>
        /// Supprime un utilisateur.
        /// </summary>
        public async Task DeleteUserAsync(User currentUser, User userToDelete)
        {
            if (currentUser.RoleId != (int)UserRole.SuperAdmin) throw new UnauthorizedAccessException("Seul le super administrateur peut supprimer des utilisateurs.");
            if (userToDelete.RoleId == (int)UserRole.SuperAdmin) throw new InvalidOperationException("Le compte du super administrateur ne peut pas être supprimé.");

            var userToRemove = await _context.Users.FindAsync(userToDelete.UserId) ?? throw new ArgumentException("Utilisateur introuvable.");

            _context.Users.Remove(userToRemove);
            await _context.SaveChangesAsync();

            OnMessageSent($"Utilisateur '{userToDelete.Username}' supprimé avec succès.");
        }

        public async Task DemoteToClientAsync(User currentUser, User targetUser)
        {
            if (currentUser.UserId != (int)UserRole.SuperAdmin) throw new UnauthorizedAccessException("Seul un super administrateur peut rétrograder un utilisateur en client.");
            var userToDemote = await _context.Users.FindAsync(targetUser.UserId) ?? throw new ArgumentException("Utilisateur introuvable.");
            if (userToDemote.RoleId != (int)UserRole.Client) throw new InvalidOperationException("L'utilisateur est déjà un client.");

            userToDemote.RoleId = (int)UserRole.Client;
            await _context.SaveChangesAsync();

            OnMessageSent($"L'utilisateur '{targetUser.Username}' a été rétrogradé au rôle de client.");
        }

        /// <summary>
        /// Trouve un utilisateur par son email.
        /// </summary>
        public async Task<User?> FindUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Trouve un utilisateur par son identifiant unique.
        /// </summary>
        public async Task<User?> FindUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        /// <summary>
        /// Trouve un utilisateur par son nom d'utilisateur.
        /// </summary>
        public async Task<User?> FindUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        /// <summary>
        /// Obtient tous les utilisateurs.
        /// </summary>
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Obtient les utilisateurs ayant un rôle spécifique.
        /// </summary>
        public async Task<List<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _context.Users.AsNoTracking().Where(u => u.RoleId == (int)role).ToListAsync();
        }

        /// <summary>
        /// Déconnecte l'utilisateur actuel.
        /// </summary>
        public void Logout()
        {
            CurrentUser = new User();
        }

        /// <summary>
        /// Change l'état actif d'un utilisateur.
        /// </summary>
        public async Task MakeNotActiveAsync(User user)
        {
            var userToUpdate = await _context.Users.FindAsync(user.UserId) ?? throw new ArgumentException("Utilisateur introuvable.");

            userToUpdate.IsActive = user.IsActive == 1 ? 0 : 1;
            await _context.SaveChangesAsync();

            OnMessageSent($"Le statut actif de l'utilisateur '{user.Username}' a été mis à jour.");
        }

        public async Task PromoteToAdminAsync(User currentUser, User targetUser)
        {
            if (currentUser.RoleId != (int)UserRole.SuperAdmin) throw new UnauthorizedAccessException("Seul un super administrateur peut promouvoir un utilisateur en administrateur.");
            var userToPromote = await _context.Users.FindAsync(targetUser.UserId) ?? throw new ArgumentException("Utilisateur introuvable.");
            if (userToPromote.RoleId == (int)UserRole.Administrator) throw new InvalidOperationException("L'utilisateur est déjà administrateur.");

            userToPromote.RoleId = (int)UserRole.Administrator;
            await _context.SaveChangesAsync();

            OnMessageSent($"L'utilisateur '{targetUser.Username}' a été promu au rôle d'administrateur.");
        }

        public async Task PromoteToLibrarianAsync(User currentUser, User targetUser)
        {
            if (currentUser.RoleId != (int)UserRole.SuperAdmin) throw new UnauthorizedAccessException("Seul un super administrateur peut promouvoir un utilisateur en bibliothécaire.");
            var userToPromote = await _context.Users.FindAsync(targetUser.UserId) ?? throw new ArgumentException("Utilisateur introuvable.");
            if (userToPromote.RoleId == (int)UserRole.Librarian) throw new InvalidOperationException("L'utilisateur est déjà bibliothécaire.");

            userToPromote.RoleId = (int)UserRole.Librarian;
            await _context.SaveChangesAsync();

            OnMessageSent($"L'utilisateur '{targetUser.Username}' a été promu au rôle de bibliothécaire.");
        }

        /// <summary>
        /// Vérifie si un utilisateur existe en fonction du nom d'utilisateur ou de l'email.
        /// </summary>
        private async Task<bool> UserExistsAsync(string username, string email)
        {
            return await _context.Users.AnyAsync(u => u.Username == username || u.Email == email);
        }

        /// <summary>
        /// Enregistre un nouvel utilisateur avec le rôle de client par défaut.
        /// </summary>
        public async Task RegisterUserAsync(string username, string password, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Tous les champs sont requis.");

            if (await UserExistsAsync(username, email))
                throw new InvalidOperationException("Un utilisateur avec ce nom d'utilisateur ou cet email existe déjà.");

            var newUser = new User(username, password, email);
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Met à jour l'email d'un utilisateur.
        /// </summary>
        public async Task UpdateEmailAsync(User user, string newEmail)
        {
            if (await _context.Users.AnyAsync(u => u.Email == newEmail)) throw new InvalidOperationException("Cet email est déjà utilisé.");

            var userToUpdate = await _context.Users.FindAsync(user.UserId) ?? throw new ArgumentException("Utilisateur introuvable.");

            userToUpdate.Email = newEmail;
            await _context.SaveChangesAsync();

            OnMessageSent($"Email de l'utilisateur '{user.Username}' mis à jour en {newEmail}.");
        }

        /// <summary>
        /// Met à jour le mot de passe d'un utilisateur.
        /// </summary>
        public async Task UpdatePasswordAsync(User user, string newPassword)
        {
            var userToUpdate = await _context.Users.FindAsync(user.UserId) ?? throw new ArgumentException("Utilisateur introuvable");
            userToUpdate.SetPassword(newPassword);
            await _context.SaveChangesAsync();

            OnMessageSent($"Mot de passe de l'utilisateur '{user.Username}' mis à jour.");
        }

        /// <summary>
        /// Met à jour le nom d'utilisateur d'un utilisateur.
        /// </summary>
        public async Task UpdateUsernameAsync(User user, string newUsername)
        {
            if (await _context.Users.AnyAsync(u => u.Username == newUsername)) throw new InvalidOperationException("Ce nom d'utilisateur est déjà pris.");

            var userToUpdate = await _context.Users.FindAsync(user.UserId) ?? throw new ArgumentException("Utilisateur introuvable");

            userToUpdate.Username = newUsername;
            await _context.SaveChangesAsync();

            OnMessageSent($"Nom d'utilisateur mis à jour en '{newUsername}'.");
        }
    }
}
