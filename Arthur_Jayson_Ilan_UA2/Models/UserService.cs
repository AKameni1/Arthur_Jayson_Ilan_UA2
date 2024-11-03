using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public class UserService
    {
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users => _users;

        public event EventHandler<string>? MessageSent;

        protected void OnMessageSent(string message)
        {
            MessageSent?.Invoke(this, message);
        }

        public UserService()
        {
            _users = new ObservableCollection<User>();
            InitializeUsers();

            _users.CollectionChanged += Users_CollectionChanged;

            int lastUserId = _users.Any() ? _users.Max(u => u.UserID) : 0;
            IdGenerator.Initialize(lastUserId);
        }
        private void InitializeUsers()
        {
            // Initialiser les utilisateurs

            // Ajout le super administrateur
            if (!_users.Any(u => u.Role == UserRole.SuperAdmin))
                RegisterSuperAdmin("SuperAdmin", "SuperPassword", "admin@example.com");

            // Ajout d'autres utilisateurs par défaut si nécessaire
            RegisterUser("User1", "Password1", "user1@example.com");
            RegisterUser("User2", "Password2", "user2@example.com");
        }

        private void Users_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (User newUser in e.NewItems)
                {
                    // Souscrire à l'événement PropretyChanged du nouvel utilisateur
                    newUser.PropertyChanged += User_PropertyChanged;

                    // Actions supplémentaires si nécessaire
                    OnMessageSent($"Nouvel utilisateur ajouté : {newUser.Username}");
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (User oldUser in e.OldItems)
                {
                    // Se désabonner de l'événement PropertyChanged de l'utilisateur supprimé
                    oldUser.PropertyChanged -= User_PropertyChanged;

                    // Actions supplémentaires si nécessaire
                    OnMessageSent($"Utilisateur supprimé : {oldUser.Username}");
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace && e.NewItems != null && e.OldItems != null)
            {
                foreach (User oldUser in e.OldItems)
                {
                    // Se désabonner de l'événement PropertyChanged de l'ancien utilisateur
                    oldUser.PropertyChanged -= User_PropertyChanged;
                }
                foreach (User newUser in e.NewItems)
                {
                    // Souscrire à l'événement PropertyChanged du nouvel utilisateur
                    newUser.PropertyChanged += User_PropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                // La collection a été réinitialisée
                // Se désabonner de tous les utilisateurs précédents
                foreach (var user in _users)
                {
                    user.PropertyChanged -= User_PropertyChanged;
                }

                // Souscrire aux utilisateurs actuels
                foreach (var user in _users)
                {
                    user.PropertyChanged += User_PropertyChanged;
                }
            }
        }

        private void User_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not User user)
                return;

            string? message = e.PropertyName switch
            {
                nameof(User.Role) => $"L'utilisateur {user.Username} a maintenant le rôle {user.Role}",
                nameof(User.Email) => $"L'utilisateur {user.Username} a changé son email en {user.Email}",
                nameof(User.Username) => $"L'utilisateur a changé son nom d'utilisateur en {user.Username}",
                // Gérer d'autres propriétés si nécessaire
                _ => $"Propriété {e.PropertyName} de l'utilisateur {user.Username} a changé.",
            };
            if (message != null)
            {
                // Envoyer le message à la vue
                OnMessageSent(message);
            }
        }

        /// <summary>
        /// Enregistre le super administrateur.
        /// </summary>
        private void RegisterSuperAdmin(string username, string password, string email)
        {
            if (_users.Any(u => u.Role == UserRole.SuperAdmin))
                throw new InvalidOperationException("Un super administrateur existe déjà");

            int userId = IdGenerator.GetNextUserId();
            User superAdmin = new User(userId, username, email, password, UserRole.SuperAdmin, true);

            _users.Add(superAdmin);
        }

        /// <summary>
        /// Enregistre un nouvel utilisateur avec le rôle de client par défaut.
        /// </summary>
        public void RegisterUser(string username, string password, string email)
        {
            if (UsernameExists(username))
                throw new InvalidOperationException("Un utilisateur avec ce nom d'utilisateur existe déjà.");

            int userId = IdGenerator.GetNextUserId();
            User newUser = new User(userId, username, email, password);

            _users.Add(newUser);
        }

        /// <summary>
        /// Authentifie un utilisateur en vérifiant le nom d'utilisateur et le mot de passe.
        /// </summary>
        public User? Authenticate(string username, string password)
        {
            User? user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user != null && user.VerifyPassword(password))
            {
                return user;
            }

            return null;
        }

        /// <summary>
        /// Vérifie si un nom d'utilisateur existe déjà.
        /// </summary>
        public bool UsernameExists(string username)
        {
            return _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Trouve un utilisateur par son identifiant unique.
        /// </summary>
        public User? FindUserById(int userId)
        {
            return _users.FirstOrDefault(u => u.UserID == userId);
        }

        /// <summary>
        /// Trouve un utilisateur par son nom d'utilisateur.
        /// </summary>
        public User? FindUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Trouve un utilisateur par son email.
        /// </summary>
        public User? FindUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Met à jour le mot de passe d'un utilisateur.
        /// </summary>
        public void UpdatePassword(User user, string newPassword)
        {
            user.ChangePassword(newPassword);
        }

        /// <summary>
        /// Met à jour l'email d'un utilisateur.
        /// </summary>
        public void UpdateEmail(User user, string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("L'email ne peut pas être vide ou null.");

            user.Email = newEmail;
        }

        /// <summary>
        /// Met à jour le nom d'utilisateur d'un utilisateur.
        /// </summary>
        public void UpdateUsername(User user, string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername))
                throw new ArgumentException("Le nom d'utilisateur ne peut pas être vide ou null.");

            if (UsernameExists(newUsername))
                throw new InvalidOperationException("Un utilisateur avec ce nom d'utilisateur existe déjà.");

            user.Username = newUsername;
        }

        /// <summary>
        /// Promeut un utilisateur au rôle d'administrateur.
        /// </summary>
        public void PromoteToAdmin(User currentUser, User targetUser)
        {
            if (!currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Seul le super administrateur peut promouvoir des utilisateurs.");

            if (targetUser.IsSuperAdmin)
                throw new InvalidOperationException("Le super administrateur ne peut pas être promu.");

            if (targetUser.Role == UserRole.Administrator)
                throw new InvalidOperationException("L'utilisateur est déjà administrateur.");

            if (_users.Count(u => u.Role == UserRole.Administrator) >= 4)
                throw new InvalidOperationException("Nombre maximal d'administrateurs atteint.");

            targetUser.ChangeRole(UserRole.Administrator);
        }

        /// <summary>
        /// Promeut un utilisateur au rôle de bibliothécaire.
        /// </summary>
        public void PromoteToLibrarian(User currentUser, User targetUser)
        {
            if (!currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Seul le super administrateur peut promouvoir des utilisateurs.");

            if (targetUser.IsSuperAdmin)
                throw new InvalidOperationException("Le super administrateur ne peut pas être promu.");

            if (targetUser.Role == UserRole.Librarian)
                throw new InvalidOperationException("L'utilisateur est déjà bibliothécaire.");

            if (_users.Count(u => u.Role == UserRole.Librarian) >= 3)
                throw new InvalidOperationException("Nombre maximal de bibliothécaires atteint.");

            targetUser.ChangeRole(UserRole.Librarian);
        }

        /// <summary>
        /// Rétrograde un utilisateur au rôle de client.
        /// </summary>
        public void DemoteToClient(User currentUser, User targetUser)
        {
            if (!currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Seul le super administrateur peut rétrograder des utilisateurs.");

            if (targetUser.IsSuperAdmin)
                throw new InvalidOperationException("Le super administrateur ne peut pas être rétrogradé.");

            if (targetUser.Role == UserRole.Client)
                throw new InvalidOperationException("L'utilisateur est déjà client.");

            targetUser.ChangeRole(UserRole.Client);
        }

        /// <summary>
        /// Supprime un utilisateur.
        /// </summary>
        public void DeleteUser(User currentUser, User userToDelete)
        {
            if (!currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Seul le super administrateur peut supprimer des utilisateurs.");

            if (userToDelete.IsSuperAdmin)
                throw new InvalidOperationException("Le compte du super administrateur ne peut pas être supprimé.");

            bool removed = _users.Remove(userToDelete);

            if (!removed)
                throw new ArgumentException("L'utilisateur spécifié n'existe pas.");
        }

        /// <summary>
        /// Obtient tous les utilisateurs.
        /// </summary>
        public ObservableCollection<User> GetAllUsers()
        {
            return _users;
        }

        /// <summary>
        /// Obtient les utilisateurs ayant un rôle spécifique.
        /// </summary>
        public ObservableCollection<User> GetUsersByRole(UserRole role)
        {
            var usersByRole = _users.Where(u => u.Role == role);
            return new ObservableCollection<User>(usersByRole);
        }

        /// <summary>
        /// Compte le nombre d'utilisateurs par rôle.
        /// </summary>
        public int CountUsersByRole(UserRole role)
        {
            return _users.Count(u => u.Role == role);
        }

        // Autres méthodes pour gérer les utilisateurs
    }
}
