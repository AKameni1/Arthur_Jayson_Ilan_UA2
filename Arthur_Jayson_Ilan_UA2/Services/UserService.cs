using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Data;
using Arthur_Jayson_Ilan_UA2.Models;
using Microsoft.EntityFrameworkCore;

namespace Arthur_Jayson_Ilan_UA2.Services
{
    public class UserService : IUserService
    {
        private readonly LibraryContext _context;

        public event EventHandler<string>? MessageSent;

        protected void OnMessageSent(string message)
        {
            MessageSent?.Invoke(this, message);
        }

        public User CurrentUser { get; private set; } = new User();

        public UserService(LibraryContext context)
        {
            _context = context;
        }

        #region Registration

        /// <summary>
        /// Enregistre le super administrateur.
        /// </summary>
        public async Task<User> RegisterSuperAdminAsync(string username, string password, string email)
        {
            if (await _context.Users.AnyAsync(u => u.Role == UserRole.SuperAdmin))
                throw new InvalidOperationException("Un super administrateur existe déjà.");

            if (await UsernameExistsAsync(username))
                throw new InvalidOperationException("Un utilisateur avec ce nom d'utilisateur existe déjà.");

            if (await EmailExistsAsync(email))
                throw new InvalidOperationException("Un utilisateur avec cette adresse e-mail existe déjà.");

            var superAdmin = new User(username, email, password, UserRole.SuperAdmin, true);

            _context.Users.Add(superAdmin);
            await _context.SaveChangesAsync();

            OnMessageSent($"Super administrateur '{superAdmin.Username}' enregistré avec succès.");

            return superAdmin;
        }

        /// <summary>
        /// Enregistre un nouvel utilisateur avec le rôle de client par défaut.
        /// </summary>
        public async Task<User> RegisterUserAsync(string username, string password, string email)
        {
            if (await UsernameExistsAsync(username))
                throw new InvalidOperationException("Un utilisateur avec ce nom d'utilisateur existe déjà.");

            if (await EmailExistsAsync(email))
                throw new InvalidOperationException("Un utilisateur avec cette adresse e-mail existe déjà.");

            User newUser = new(username, email, password);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            OnMessageSent($"Utilisateur '{newUser.Username}' enregistré avec succès.");

            return newUser;
        }

        #endregion


        #region Authentication

        /// <summary>
        /// Authentifie un utilisateur en vérifiant le nom d'utilisateur et le mot de passe.
        /// </summary>
        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users
                                     .Include(u => u.RoleEntity)
                                     .FirstOrDefaultAsync(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user != null && user.VerifyPassword(password))
            {
                CurrentUser = user;
                CurrentUser.IsSuperAdmin = user.RoleEntity != null && user.RoleEntity.Name.Equals("superAdmin", StringComparison.OrdinalIgnoreCase);
                OnMessageSent($"Utilisateur '{user.Username}' authentifié avec succès.");
                return user;
            }

            OnMessageSent("Échec de l'authentification. Nom d'utilisateur ou mot de passe incorrect.");
            return null;
        }

        #endregion

        #region User Retrieval

        public async Task<User?> FindUserByIdAsync(int userId)
        {
            return await _context.Users
                                 .Include(u => u.RoleEntity)
                                 .FirstOrDefaultAsync(u => u.UserID == userId);
        }

        public async Task<User?> FindUserByUsernameAsync(string username)
        {
            return await _context.Users
                                 .FirstOrDefaultAsync(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            return await _context.Users
                                 .FirstOrDefaultAsync(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region Update Methods

        /// <summary>
        /// Met à jour le mot de passe d'un utilisateur.
        /// </summary>
        public async Task UpdatePasswordAsync(User user, string newPassword)
        {
            user.SetPassword(newPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            OnMessageSent($"Mot de passe de l'utilisateur '{user.Username}' mis à jour.");
        }

        /// <summary>
        /// Met à jour l'email d'un utilisateur.
        /// </summary>
        public async Task UpdateEmailAsync(User user, string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail) || !Regex.IsMatch(newEmail, @"^(?!.*\.\.)[a-zA-Z0-9](?:[a-zA-Z0-9._-]*[a-zA-Z0-9])?@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.[a-zA-Z]{2,6}$"))
                throw new ArgumentException("L'email est invalide.");

            if (await EmailExistsAsync(newEmail))
                throw new InvalidOperationException("Un utilisateur avec cette adresse e-mail existe déjà.");

            user.Email = newEmail;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            OnMessageSent($"Email de l'utilisateur '{user.Username}' mis à jour en {newEmail}.");
        }

        /// <summary>
        /// Met à jour le nom d'utilisateur d'un utilisateur.
        /// </summary>
        public async Task UpdateUsernameAsync(User user, string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername))
                throw new ArgumentException("Le nom d'utilisateur ne peut pas être vide ou null.");

            if (await UsernameExistsAsync(newUsername))
                throw new InvalidOperationException("Un utilisateur avec ce nom d'utilisateur existe déjà.");

            user.Username = newUsername;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            OnMessageSent($"Nom d'utilisateur mis à jour en '{newUsername}'.");
        }

        #endregion

        #region Role Management

        /// <summary>
        /// Promeut un utilisateur au rôle d'administrateur.
        /// </summary>
        public async Task PromoteToAdminAsync(User currentUser, User targetUser)
        {
            if (!currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Seul le super administrateur peut promouvoir des utilisateurs.");

            if (targetUser.IsSuperAdmin)
                throw new InvalidOperationException("Le super administrateur ne peut pas être promu.");

            if (targetUser.Role == UserRole.Administrator)
                throw new InvalidOperationException("L'utilisateur est déjà administrateur.");

            int adminCount = await _context.Users.CountAsync(u => u.Role == UserRole.Administrator);
            if (adminCount >= 4)
                throw new InvalidOperationException("Nombre maximal d'administrateurs atteint.");

            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleID == (int)UserRole.Administrator);
            if (adminRole == null)
                throw new InvalidOperationException("Le rôle 'admin' n'existe pas dans la base de données.");

            targetUser.Role = UserRole.Administrator;
            _context.Users.Update(targetUser);
            await _context.SaveChangesAsync();

            OnMessageSent($"L'utilisateur '{targetUser.Username}' a été promu au rôle d'administrateur.");
        }

        /// <summary>
        /// Promeut un utilisateur au rôle de bibliothécaire.
        /// </summary>
        public async Task PromoteToLibrarianAsync(User currentUser, User targetUser)
        {
            if (!currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Seul le super administrateur peut promouvoir des utilisateurs.");

            if (targetUser.IsSuperAdmin)
                throw new InvalidOperationException("Le super administrateur ne peut pas être promu.");

            if (targetUser.Role == UserRole.Librarian)
                throw new InvalidOperationException("L'utilisateur est déjà bibliothécaire.");

            int librarianCount = await _context.Users.CountAsync(u => u.Role == UserRole.Librarian);
            if (librarianCount >= 3)
                throw new InvalidOperationException("Nombre maximal de bibliothécaires atteint.");

            targetUser.Role = UserRole.Librarian;
            _context.Users.Update(targetUser);
            await _context.SaveChangesAsync();

            OnMessageSent($"L'utilisateur '{targetUser.Username}' a été promu au rôle de bibliothécaire.");
        }

        /// <summary>
        /// Rétrograde un utilisateur au rôle de client.
        /// </summary>
        public async Task DemoteToClientAsync(User currentUser, User targetUser)
        {
            if (!currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Seul le super administrateur peut rétrograder des utilisateurs.");

            if (targetUser.IsSuperAdmin)
                throw new InvalidOperationException("Le super administrateur ne peut pas être rétrogradé.");

            if (targetUser.Role == UserRole.Client)
                throw new InvalidOperationException("L'utilisateur est déjà client.");

            targetUser.Role = UserRole.Client;
            _context.Users.Update(targetUser);
            await _context.SaveChangesAsync();

            OnMessageSent($"L'utilisateur '{targetUser.Username}' a été rétrogradé au rôle de client.");
        }

        #endregion

        #region Deletion

        /// <summary>
        /// Supprime un utilisateur.
        /// </summary>
        public async Task DeleteUserAsync(User currentUser, User userToDelete)
        {
            if (!currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Seul le super administrateur peut supprimer des utilisateurs.");

            if (userToDelete.IsSuperAdmin)
                throw new InvalidOperationException("Le compte du super administrateur ne peut pas être supprimé.");

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            OnMessageSent($"Utilisateur '{userToDelete.Username}' supprimé avec succès.");
        }

        #endregion

        #region Retrieval

        /// <summary>
        /// Obtient tous les utilisateurs.
        /// </summary>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                                 .Include(u => u.RoleEntity)
                                 .ToListAsync();
        }

        /// <summary>
        /// Obtient les utilisateurs ayant un rôle spécifique.
        /// </summary>
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _context.Users
                                 .Where(u => u.Role == role)
                                 .Include(u => u.RoleEntity)
                                 .ToListAsync();
        }

        /// <summary>
        /// Compte le nombre d'utilisateurs par rôle.
        /// </summary>
        public async Task<int> CountUsersByRoleAsync(UserRole role)
        {
            return await _context.Users
                                 .CountAsync(u => u.Role == role);
        }

        #endregion

        #region Toggle Active Status

        public async Task ToggleUserActiveStatusAsync(User user)
        {
            user.IsActive = !user.IsActive;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            OnMessageSent($"Le statut actif de l'utilisateur '{user.Username}' a été mis à jour.");
        }

        #endregion

        #region Logout

        public Task LogoutAsync()
        {
            CurrentUser = new User();
            OnMessageSent("Utilisateur déconnecté.");
            return Task.CompletedTask;
        }

        #endregion

        #region Helper Methods

        private async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
