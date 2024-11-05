using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Arthur_Jayson_Ilan_UA2.Commands;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Views.UserManagementViews;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Arthur_Jayson_Ilan_UA2.Models.SubMenu;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels
{
    public class UserManagementViewModel : INotifyPropertyChanged
    {
        private UserControl _currentView = new();
        private UserManagementSubMenu _selectedSubMenu;
        public UserControl CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public UserManagementSubMenu SelectedSubMenu
        {
            get => _selectedSubMenu;
            set { _selectedSubMenu = value; OnPropertyChanged(); }
        }

        public ICommand ShowAccountAdministrationCommand { get; }
        public ICommand ShowRoleAssignmentCommand { get; }

        public UserManagementViewModel()
        {
            // Initialiser les commandes
            ShowAccountAdministrationCommand = new RelayCommand(ExecuteShowAccountAdministration);
            ShowRoleAssignmentCommand = new RelayCommand(ExecuteShowRoleAssignment);

            // Définir la vue par défaut
            CurrentView = new AccountAdministrationView();
        }

        private void ExecuteShowAccountAdministration(object? parameter)
        {
            CurrentView = new AccountAdministrationView();
            SelectedSubMenu = UserManagementSubMenu.AccountAdministration;
        }

        private void ExecuteShowRoleAssignment(object? parameter)
        {
            CurrentView = new RoleAssignmentView();
            SelectedSubMenu = UserManagementSubMenu.RoleAssignment;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
