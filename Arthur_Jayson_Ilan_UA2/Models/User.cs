using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public class User : INotifyPropertyChanged
    {
        // Champs privés
        private int _userID;
        private string _username = string.Empty;
        private string _email = string.Empty;
        private string _passwordHash = string.Empty;
        private DateTime _creationDate;
        private bool _isActive;
        private UserRole _role;
        private bool _isSuperAdmin;
        private bool _isSelected;

        private static Random _random = new Random();

        // Propriétés publiques avec notifications de changement

        public int UserID
        {
            get => _userID;
            set
            {
                if (_userID != value)
                {
                    _userID = value;
                    OnPropertyChanged(nameof(UserID));
                }
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string PasswordHash
        {
            get => _passwordHash;
            private set
            {
                if (_passwordHash != value)
                {
                    _passwordHash = value;
                    OnPropertyChanged(nameof(PasswordHash));
                }
            }
        }

        public UserRole Role
        {
            get => _role;
            set
            {
                if (_role != value)
                {
                    _role = value;
                    OnPropertyChanged(nameof(Role));
                }
            }
        }

        public bool IsSuperAdmin
        {
            get => _isSuperAdmin;
            set
            {
                if (_isSuperAdmin != value)
                {
                    _isSuperAdmin = value;
                    OnPropertyChanged(nameof(IsSuperAdmin));
                }
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged(nameof(IsActive));
                }
            }
        }

        public DateTime CreationDate
        {
            get => _creationDate;
            set
            {
                if (_creationDate != value)
                {
                    _creationDate = value;
                    OnPropertyChanged(nameof(CreationDate));
                }
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set 
            {
                // Prevent SuperAdmin users from being selected
                if (Role == UserRole.SuperAdmin)
                {
                    _isSelected = false;
                }
                else
                {
                    if (_isSelected != value)
                    {
                        _isSelected = value;
                        OnPropertyChanged(nameof(IsSelected));
                    }
                }
                

            }
        }

        // Constructeur avec paramètres
        public User(int userID, string username, string email, string password, UserRole role = UserRole.Client, bool isSuperAdmin = false, bool isActive = true, DateTime? creationDate = null)
        {
            UserID = userID;
            Username = username;
            Email = email;
            SetPassword(password);
            Role = role;
            IsSuperAdmin = isSuperAdmin;
            IsActive = isActive;
            CreationDate = creationDate ?? DateTime.Now;
        }

        // Constructeur avec 1 seul paramètre
        public User(int userID) 
        {
            UserID = userID;
            Username = $"User{userID}";
            Email = $"{Username.ToLower()}@example.com";
            SetPassword(GenerateRandomPasswordHash);
            Role = UserRole.Client;
            IsSuperAdmin = false;
            IsActive = true;
            CreationDate = DateTime.Now;
        }

        // Constructeur sans paramètres
        public User() { }

        // Méthode pour afficher la date formatée (facultatif)
        public string GetFormattedCreationDate()
        {
            return CreationDate.ToString("f");
        }

        // Méthode pour générer un mot de passe hashé aléatoire (simulée ici)
        private static string GenerateRandomPasswordHash
        {
            get
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var password = new char[10];
                for (int i = 0; i < password.Length; i++)
                {
                    password[i] = chars[_random.Next(chars.Length)];
                }
                return new string(password);
            }
        }

        // Méthodes

        /// <summary>
        /// Définit le mot de passe en le hachant avec BCrypt.
        /// </summary>
        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Le mot de passe ne peut pas être vide.");

            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Vérifie si le mot de passe fourni correspond au mot de passe haché de l'utilisateur.
        /// </summary>
        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrEmpty(PasswordHash))
                throw new InvalidOperationException("Le mot de passe n'a pas été défini.");

            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        /// <summary>
        /// Change le rôle de l'utilisateur.
        /// </summary>
        public void ChangeRole(User currentUser, User targetUser, UserRole newRole)
        {
            if (currentUser == targetUser && currentUser.Role == UserRole.SuperAdmin)
                throw new InvalidOperationException("Le SuperAdmin ne peut pas modifier son propre rôle.");

            if (IsSuperAdmin)
                throw new InvalidOperationException("Le super administrateur ne peut pas changer son propre rôle.");

            if (newRole == UserRole.SuperAdmin)
                throw new ArgumentException("Le rôle 'SuperAdmin' ne peut pas être attribué.");

            Role = newRole;
        }

        /// <summary>
        /// Change le mot de passe de l'utilisateur en le hachant.
        /// </summary>
        public void ChangePassword(string newPassword)
        {
            SetPassword(newPassword);
        }

        /// <summary>
        /// Vérifie si l'utilisateur est un administrateur.
        /// </summary>
        public bool IsAdmin() => Role == UserRole.Administrator;

        /// <summary>
        /// Vérifie si l'utilisateur est un bibliothécaire.
        /// </summary>
        public bool IsLibrarian() => Role == UserRole.Librarian;

        /// <summary>
        /// Surcharge de la méthode Equals pour comparer les utilisateurs.
        /// </summary>
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

        // Implémentation de INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
