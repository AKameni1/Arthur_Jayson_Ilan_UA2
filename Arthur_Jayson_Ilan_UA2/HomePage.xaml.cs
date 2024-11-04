using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Arthur_Jayson_Ilan_UA2.Models;
using Arthur_Jayson_Ilan_UA2.Services;
using Arthur_Jayson_Ilan_UA2.Views;
using Arthur_Jayson_Ilan_UA2.ViewsModels;

namespace Arthur_Jayson_Ilan_UA2
{
    /// <summary>
    /// Logique d'interaction pour HomePage.xaml
    /// </summary>
    public partial class HomePage : Window, INavigableWindow
    {
        private bool IsMaximized = false;
        public HomePage()
        {
            InitializeComponent();
            DataContext = new HomePageViewModel();

            MainContentControl.Content = new DashboardView();

            // Définir cette fenêtre comme fenêtre active dans NavigationService
            NavigationService.Instance.SetCurrentWindow(this);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized = true;
                }
            }
        }


        public void LoadNewUserControl(UserControl view)
        {
            MainContentControl.Content = view;
        }

        public void OnNavigatedTo(object parameter)
        {
            if (DataContext is HomePageViewModel viewModel)
            {
                viewModel.OnNavigatedTo(parameter);
            }
        }
    }
}
