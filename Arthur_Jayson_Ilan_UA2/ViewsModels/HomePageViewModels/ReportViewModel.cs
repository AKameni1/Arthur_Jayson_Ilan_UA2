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
using Arthur_Jayson_Ilan_UA2.Views.HomePageViews.ReportViews;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.HomePageViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        private UserControl _currentView = new();
        private ReportSubMenu _selectedSubMenu;
        public UserControl CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ReportSubMenu SelectedSubMenu
        {
            get => _selectedSubMenu;
            set { _selectedSubMenu = value; OnPropertyChanged(); }
        }

        public ICommand ShowUsageStatisticCommand { get; }
        public ICommand ShowFinancialReportCommand { get; }

        public ReportViewModel()
        {
            // Initialiser les commandes
            ShowUsageStatisticCommand = new RelayCommand(ExecuteShowUsageStatistic);
            ShowFinancialReportCommand = new RelayCommand(ExecuteShowFinancialReport);

            // Définir la vue par défaut
            CurrentView = new UsageStatistic();
        }

        private void ExecuteShowUsageStatistic(object? parameter)
        {
            CurrentView = new UsageStatistic();
            SelectedSubMenu = ReportSubMenu.UsageStatistic;
        }

        private void ExecuteShowFinancialReport(object? parameter)
        {
            CurrentView = new FinancialReport();
            SelectedSubMenu = ReportSubMenu.FinancialReport;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
