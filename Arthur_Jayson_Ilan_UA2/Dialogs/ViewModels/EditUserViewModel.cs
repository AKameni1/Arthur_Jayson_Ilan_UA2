using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Models;

namespace Arthur_Jayson_Ilan_UA2.Dialogs.ViewModels
{
    public class EditUserViewModel : INotifyPropertyChanged
    {
        private User _userToEdit = new User();
        public User UserToEdit
        {
            get => _userToEdit;
            set
            {
                if (_userToEdit != value)
                {
                    _userToEdit = value;
                    OnPropertyChanged(nameof(UserToEdit));
                }
            }
        }

        public EditUserViewModel(User userToEdit)
        {
            UserToEdit = userToEdit;
        }




        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
