using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public class Permission : INotifyPropertyChanged
    {
        // Champs privés
        private int _permissionID;
        private string _name = string.Empty;
        private string _description = string.Empty;

        // Propriétés publiques avec notifications de changement

        [Key]
        public int PermissionID
        {
            get => _permissionID;
            set
            {
                if (_permissionID != value)
                {
                    _permissionID = value;
                    OnPropertyChanged(nameof(PermissionID));
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
        public ICollection<UserPermission>? UserPermissions { get; set; }

        // Constructeurs
        public Permission() { }

        public Permission(int permissionID, string name, string description)
        {
            PermissionID = permissionID;
            Name = name;
            Description = description;
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
