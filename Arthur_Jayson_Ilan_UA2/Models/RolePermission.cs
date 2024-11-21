using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    [Table("rolepermission")]
    public class RolePermission : INotifyPropertyChanged
    {

        // Champs privés
        private int _roleID;
        private int _permissionID;

        // Propriétés publiques avec notifications de changement

        [Key, Column(Order = 0)]
        [ForeignKey("Role")]
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

        public Role? Role { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Permission")]
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

        public Permission? Permission { get; set; }

        // Constructeurs
        public RolePermission() { }

        public RolePermission(int roleID, int permissionID)
        {
            RoleID = roleID;
            PermissionID = permissionID;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
