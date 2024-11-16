using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public class Role : INotifyPropertyChanged
    {
        // Champs privés
        private int _roleID;
        private string _name = string.Empty;
        private string _description = string.Empty;

        // Propriétés publiques avec notifications de changement

        [Key]
        public int RoleID
        {
            get => _roleID;
            set
            {
                if (_roleID != value)
                {
                    _roleID = value;
                    OnPropertyChanged(nameof(RoleID));
                }
            }
        }

        [Required]
        [MaxLength(50)]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        // Relations
        public ICollection<RolePermission>? RolePermissions { get; set; }
        public ICollection<User>? Users { get; set; }

        // Constructeurs
        public Role() { }

        public Role(int roleID, string name, string description)
        {
            RoleID = roleID;
            Name = name;
            Description = description;
        }

        // Méthodes pour ajouter ou retirer des permissions
        public void AddPermission(Permission permission)
        {
            RolePermissions ??= new List<RolePermission>();

            if (!RolePermissions.Any(rp => rp.PermissionID == permission.PermissionID))
            {
                RolePermissions.Add(new RolePermission
                {
                    RoleID = this.RoleID,
                    PermissionID = permission.PermissionID,
                    Permission = permission
                });
            }
        }

        public void RemovePermission(Permission permission)
        {
            if (RolePermissions != null)
            {
                var rolePermission = RolePermissions.FirstOrDefault(rp => rp.PermissionID == permission.PermissionID);
                if (rolePermission != null)
                {
                    RolePermissions.Remove(rolePermission);
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
