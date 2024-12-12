using System;
using System.Collections.Generic;
using Arthur_Jayson_Ilan_UA2.Services;



namespace Arthur_Jayson_Ilan_UA2.Model
{
    public partial class User : BaseModel
    {
        public int UserId { get; }

        private int _roleId;
        public int RoleId
        {
            get => _roleId;
            set
            {
                if (_roleId != value)
                {
                    _roleId = value;
                    OnPropertyChanged(nameof(RoleId));
                }
            }
        }


        private string _username = string.Empty;
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


        private string _email = string.Empty;
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


        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            private set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private string _profileImage = string.Empty;
        public string ProfileImage
        {
            get => _profileImage;
            set
            {
                if (value != _profileImage)
                {
                    _profileImage = value;
                    OnPropertyChanged(nameof(ProfileImage));
                }
            }
        }

        private int _isActive;
        public int IsActive
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

        private DateTime _creationDate;
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

        private int _loanCount;
        public int LoanCount
        {
            get => _loanCount;
            set
            {
                if (_loanCount != value)
                {
                    _loanCount = value;
                    OnPropertyChanged(nameof(LoanCount));
                }
            }
        }

        private int _loanLimit;
        public int LoanLimit
        {
            get => _loanLimit;
            set
            {
                if (!_loanLimit.Equals(value))
                {
                    _loanLimit = value;
                    OnPropertyChanged(nameof(LoanLimit));
                }
            }
        }

        private int _lateReturnCount;
        public int LateReturnCount
        {
            get => _lateReturnCount;
            set
            {
                if (!_lateReturnCount.Equals(value))
                {
                    _lateReturnCount = value;
                    OnPropertyChanged(nameof(LateReturnCount));
                }
            }
        }

        private int _penaltyPoints;
        public int PenaltyPoints
        {
            get => _penaltyPoints;
            set
            {
                if (value != _penaltyPoints)
                {
                    _penaltyPoints = value;
                    OnPropertyChanged(nameof(PenaltyPoints));
                }
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (RoleId == (int)UserRole.SuperAdmin)
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

        private bool _isSuperAdmin;
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

        public DateTime? LoanSuspendedUntil { get; set; }

        public virtual ICollection<Auditlog> Auditlogs { get; set; } = [];

        public virtual ICollection<Loan> Loans { get; set; } = [];

        public virtual ICollection<Notification> Notifications { get; set; } = [];

        public virtual ICollection<Report> Reports { get; set; } = [];

        public virtual ICollection<Reservation> Reservations { get; set; } = [];

        public virtual Role Role { get; set; } = null!;

        public virtual ICollection<Supportticket> Supporttickets { get; set; } = [];

        public virtual ICollection<Ticketresponse> Ticketresponses { get; set; } = [];

        public virtual ICollection<Permission> Permissions { get; set; } = [];

        public User(string username, string email, string password, UserRole role = UserRole.Client, bool isSuperAdmin = false, bool isActive = true, DateTime? creationDate = null)
        {
            Username = username;
            Email = email;
            SetPassword(password);
            RoleId = (int)role;
            IsSuperAdmin = isSuperAdmin;
            IsActive = isActive ? 1 : 0;
            CreationDate = creationDate ?? DateTime.Now;
        }

        // Constructeur par défaut
        public User() { }


        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Le mot de passe ne peut pas être vide.");

            Password = BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrEmpty(Password))
                throw new InvalidOperationException("Le mot de passe n'a pas été défini.");

            return BCrypt.Net.BCrypt.Verify(password, Password);
        }

        public bool IsAdmin() => RoleId == (int)UserRole.Administrator;

        public bool IsLibrarian() => RoleId == (int)UserRole.Librarian;

        public void ChangeRole(UserRole newRole)
        {
            if (newRole == UserRole.SuperAdmin)
                throw new InvalidOperationException("Le rôle 'SuperAdmin' ne peut pas être attribué.");

            RoleId = (int)newRole;
            OnPropertyChanged(nameof(RoleId));
        }

        public override bool Equals(object? obj)
        {
            if (obj is User otherUser)
            {
                return Username.Equals(otherUser.Username, StringComparison.OrdinalIgnoreCase) &&
                       Email.Equals(otherUser.Email, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Username.ToLower(), Email.ToLower());
        }

    }
}