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
using Arthur_Jayson_Ilan_UA2.Views.HomePageViews.ReservationsViews;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.HomePageViewModels
{
    public class ReservationsViewModel : INotifyPropertyChanged
    {
        private UserControl _currentView = new();
        private ReservationsSubMenu _selectedSubMenu;
        public UserControl CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ReservationsSubMenu SelectedSubMenu
        {
            get => _selectedSubMenu;
            set { _selectedSubMenu = value; OnPropertyChanged(); }
        }

        public ICommand ShowMakeReservationCommand { get; }
        public ICommand ShowReservationHistoryCommand { get; }

        public ReservationsViewModel()
        {
            // Initialiser les commandes
            ShowMakeReservationCommand = new RelayCommand(ExecuteShowMakeReservation);
            ShowReservationHistoryCommand = new RelayCommand(ExecuteShowReservationHistory);

            // Définir la vue par défaut
            CurrentView = new MakeReservation();
        }

        private void ExecuteShowMakeReservation(object? parameter)
        {
            CurrentView = new MakeReservation();
            SelectedSubMenu = ReservationsSubMenu.MakeReservation;
        }

        private void ExecuteShowReservationHistory(object? parameter)
        {
            CurrentView = new ReservationHistory();
            SelectedSubMenu = ReservationsSubMenu.ReservationHistory;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
