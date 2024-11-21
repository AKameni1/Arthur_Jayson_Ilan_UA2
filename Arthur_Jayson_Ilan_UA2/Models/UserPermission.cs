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
    [Table("userpermission")]
    public class UserPermission : INotifyPropertyChanged
    {
        // Champs privés
        private int _userID;
        private int _permissionID;

        // Propriétés publiques avec notifications de changement

        [Key, Column(Order = 0)]
        [ForeignKey("User")]
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

        public User? User { get; set; }

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
        public UserPermission() { }

        public UserPermission(int userID, int permissionID)
        {
            UserID = userID;
            PermissionID = permissionID;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
