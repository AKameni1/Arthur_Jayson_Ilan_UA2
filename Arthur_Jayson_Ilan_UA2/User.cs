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
        public string Username { get; set; } = username;
        private readonly string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        public string Email { get; set; } = email;
        public string Role { get; private set; } = role;
        public bool IsSuperAdmin { get; private set; } = isSuperAdmin;

        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrEmpty(passwordHash)) throw new InvalidOperationException("Le mot de passe n'a pas été défini.");
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public void ChangeRole(string newRole)
        {
            if (IsSuperAdmin)
            {
                throw new InvalidOperationException("Le super admin ne peut pas changer son propre rôle");
            }

            if (newRole != "client" && newRole != "administrator")
            {
                throw new ArgumentException("Rôle non valide. Les rôles possibles sont 'client' et 'administrator'.");
            }

            Role = newRole;
        }

        public bool IsAdmin()
        {
            return Role == "administrator";
        }
    }

    public class UserManager
    {
        private readonly List<User> _users = [];
        public UserManager()
        {
            RegisterSuperAdmin("SuperAdmin", "SuperPassword", "admin@example.ca");
        }

        public void RegisterSuperAdmin(string username, string password, string email)
        {
            if (_users.Exists(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))) throw new InvalidOperationException("Un super admin avec ce nom d'utilisateur existe déjà.");

            _users.Add(new User(username, password, email, role: "administrator", isSuperAdmin: true));
        }

        public void RegisterUser(string username, string password, string email)
        {
            if (_users.Exists(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))) throw new InvalidOperationException("Un utilisateur avec ce nom d'utilisateur existe déjà.");

            _users.Add(new User(username, password, email));
        }

        public User? Authenticate(string username, string password)
        {
            User? user = _users.Find(u => u.Username.Equals(username,StringComparison.OrdinalIgnoreCase));

            if (user != null && user.VerifyPassword(password)) return user;

            return user;
        }

        public static void PromoteToAdmin(User user)
        {
            if (user.IsSuperAdmin) throw new InvalidOperationException("Le super admin ne peut pas être promu.");

            user.ChangeRole("administrator");
        }
    }
}
