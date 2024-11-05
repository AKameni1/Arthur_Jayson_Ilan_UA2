using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Models.SubMenu;
using System.Windows.Controls;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Views.HomePageViews.SupportViews;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.HomePageViewModels
{
    public class SupportViewModel : INotifyPropertyChanged
    {
        private UserControl _currentView = new();
        private SupportSubMenu _selectedSubMenu;
        public UserControl CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public SupportSubMenu SelectedSubMenu
        {
            get => _selectedSubMenu;
            set { _selectedSubMenu = value; OnPropertyChanged(); }
        }

        public ICommand ShowFAQCommand { get; }
        public ICommand ShowTechnicalAssistanceCommand { get; }

        public SupportViewModel()
        {
            // Initialiser les commandes
            ShowFAQCommand = new RelayCommand(ExecuteShowFAQ);
            ShowTechnicalAssistanceCommand = new RelayCommand(ExecuteShowTechnicalAssitance);

            // Définir la vue par défaut
            CurrentView = new FAQView();
        }

        private void ExecuteShowFAQ(object? parameter)
        {
            CurrentView = new FAQView();
            SelectedSubMenu = SupportSubMenu.FAQ;
        }

        private void ExecuteShowTechnicalAssitance(object? parameter)
        {
            CurrentView = new TechnicalAssistanceView();
            SelectedSubMenu = SupportSubMenu.TechnicalAssistance;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
