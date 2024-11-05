using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Models.SubMenu;
using Arthur_Jayson_Ilan_UA2.Views.BookManagementViews;
using System.Windows.Controls;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Views.HomePageViews.LoanReturnViews;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.HomePageViewModels
{
    public class LoanReturnViewModel : INotifyPropertyChanged
    {
        private UserControl _currentView = new();
        private LoanReturnSubMenu _selectedSubMenu;
        public UserControl CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public LoanReturnSubMenu SelectedSubMenu
        {
            get => _selectedSubMenu;
            set { _selectedSubMenu = value; OnPropertyChanged(); }
        }

        public ICommand ShowLoanCommand { get; }
        public ICommand ShowReturnCommand { get; }

        public LoanReturnViewModel()
        {
            // Initialiser les commandes
            ShowLoanCommand = new RelayCommand(ExecuteShowLoan);
            ShowReturnCommand = new RelayCommand(ExecuteShowReturn);

            // Définir la vue par défaut
            CurrentView = new Loan();
        }

        private void ExecuteShowLoan(object? parameter)
        {
            CurrentView = new Loan();
            SelectedSubMenu = LoanReturnSubMenu.Loan;
        }

        private void ExecuteShowReturn(object? parameter)
        {
            CurrentView = new Return();
            SelectedSubMenu = LoanReturnSubMenu.Return;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
