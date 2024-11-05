using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Views.UserManagementViews;
using System.Windows.Controls;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Views.BookManagementViews;
using Arthur_Jayson_Ilan_UA2.Models.SubMenu;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.HomePageViewModels
{
    public class BookManagementViewModel : INotifyPropertyChanged
    {
        private UserControl _currentView = new();
        private BookManagementSubMenu _selectedSubMenu;
        public UserControl CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public BookManagementSubMenu SelectedSubMenu
        {
            get => _selectedSubMenu;
            set { _selectedSubMenu = value; OnPropertyChanged(); }
        }

        public ICommand ShowCatalogCommand { get; }
        public ICommand ShowCategorizationCommand { get; }

        public BookManagementViewModel()
        {
            // Initialiser les commandes
            ShowCatalogCommand = new RelayCommand(ExecuteShowCatalog);
            ShowCategorizationCommand = new RelayCommand(ExecuteShowCategorization);

            // Définir la vue par défaut
            CurrentView = new Catalog();
        }

        private void ExecuteShowCatalog(object? parameter)
        {
            CurrentView = new Catalog();
            SelectedSubMenu = BookManagementSubMenu.Catalog;
        }

        private void ExecuteShowCategorization(object? parameter)
        {
            CurrentView = new Categorization();
            SelectedSubMenu = BookManagementSubMenu.Categorization;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
