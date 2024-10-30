using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Arthur_Jayson_Ilan_UA2
{
    public class User(string username, string password, string email, string role = "client", bool isSuperAdmin = false)
    {
        public string Username { get; private set; } = username;
        private string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        public string Email { get; private set; } = email;
        public string Role { get; private set; } = role;
        public bool IsSuperAdmin { get; private set; } = isSuperAdmin;

        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrEmpty(passwordHash)) throw new InvalidOperationException("Le mot de passe n'a pas été défini.");
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public void ChangeRole(string newRole)
        {
            if (IsSuperAdmin) throw new InvalidOperationException("Le super admin ne peut pas changer son propre rôle");

            if (newRole != "client" && newRole != "administrator" && newRole != "bibliothecaire") throw new ArgumentException("Rôle non valide. Les rôles possibles sont 'client', 'administrator', 'bibliothecaire'.");

            Role = newRole;
        }

        public void ChangePassword(string newPassword)
        {
            passwordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        }

        public bool IsAdmin() => Role == "administrator";

        public bool IsLibrarian() => Role == "bibliothecaire";

        public void ChangeUsername(string newUsername, UserManager userManager)
        {
            if (string.IsNullOrWhiteSpace(newUsername)) throw new ArgumentException("Le nom d'utilisateur ne peut pas être vide.");

            if (userManager.UsernameExists(newUsername)) throw new InvalidOperationException("Ce nom d'utilisateur est déjà pris.");

            Username = newUsername;
        }

        public override bool Equals(object? obj)
        {
            if (obj is User otherUser)
            {
                return this.Username.Equals(otherUser.Username, StringComparison.OrdinalIgnoreCase) &&
                       this.Email.Equals(otherUser.Email, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Username.ToLower(), Email.ToLower());
        }
    }

    public class UserManager
    {
        private readonly List<User> _users = new List<User>();
        public UserManager()
        {
            RegisterSuperAdmin("SuperAdmin", "SuperPassword", "admin@example.ca");
        }

        private void RegisterSuperAdmin(string username, string password, string email)
        {
            if (_users.Exists(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))) throw new InvalidOperationException("Un super admin avec ce nom d'utilisateur existe déjà.");

            _users.Add(new User(username, password, email, role: "administrator", isSuperAdmin: true));
        }

        public void RegisterUser(string username, string password, string email)
        {
            if (UsernameExists(username)) throw new InvalidOperationException("Un utilisateur avec ce nom d'utilisateur existe déjà.");

            _users.Add(new User(username, password, email));
        }

        public User? Authenticate(string username, string password)
        {
            User? user = _users.Find(u => u.Username.Equals(username,StringComparison.OrdinalIgnoreCase));

            return user != null && user.VerifyPassword(password) ? user : null;
        }

        public User? FindUserByEmail(string email)
        {
            return _users.Find(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public void PromoteToAdmin(User currentUser, User targetuser)
        {
            if (!currentUser.IsSuperAdmin) throw new UnauthorizedAccessException("Seul le super administrateur peut promouvoir des utilisateurs.");

            if (targetuser.IsSuperAdmin) throw new InvalidOperationException("Le super admin ne peut pas être promu.");

            if (targetuser.IsAdmin()) throw new InvalidOperationException("L'utilisateur est déjà administrateur.");

            if (_users.Count(u => u.IsAdmin()) >= 4) throw new InvalidOperationException("Nombre maximal d'administrateurs atteint.");

            targetuser.ChangeRole("administrator");
        }

        public void PromoteToBibliothecaire(User currentUser, User targetuser)
        {
            if (!currentUser.IsSuperAdmin) throw new UnauthorizedAccessException("Seul le super administrateur peut promouvoir des utilisateurs.");

            if (targetuser.IsSuperAdmin) throw new InvalidOperationException("Le super admin ne peut pas être promu.");

            if (targetuser.IsLibrarian()) throw new InvalidOperationException("L'utilisateur est déjà bibliothécaire.");

            if (_users.Count(u => u.IsLibrarian()) >= 3) throw new InvalidOperationException("Nombre maximal d'administrateurs atteint.");

            targetuser.ChangeRole("bibliothecaire");
        }

        public static void DemoteToClient(User currentUser, User targetuser)
        {
            if (!currentUser.IsSuperAdmin) throw new UnauthorizedAccessException("Seul le super administrateur peut rétrograder des utilisateurs.");

            if (targetuser.IsSuperAdmin) throw new InvalidOperationException("Le super admin ne peut pas être rétrogradé.");

            if (targetuser.Role == "client") throw new InvalidOperationException("L'utilisateur est déjà client.");

            targetuser.ChangeRole("client");
        }

        public void DeleteUser(User currentUser, User userToDelete)
        {
            if (!currentUser.IsSuperAdmin) throw new UnauthorizedAccessException("Seul le super administrateur peut supprimer des utilisateurs.");

            if (userToDelete.IsSuperAdmin) throw new InvalidOperationException("Le compte du super administrateur ne peut pas être supprimé.");

            bool removed = _users.Remove(userToDelete);

            if (!removed) throw new ArgumentException("L'utilisateur spécifié n'existe pas.");
        }

        public void UpdatePassword(User user, string newPassword)
        {
            user.ChangePassword(newPassword);
        }

        public bool UsernameExists(string username)
        {
            return _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
