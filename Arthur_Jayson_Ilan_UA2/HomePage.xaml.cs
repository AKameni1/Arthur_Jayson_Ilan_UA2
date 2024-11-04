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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Arthur_Jayson_Ilan_UA2.Models;
using Arthur_Jayson_Ilan_UA2.Services;
using Arthur_Jayson_Ilan_UA2.ViewsModels;

namespace Arthur_Jayson_Ilan_UA2
{
    /// <summary>
    /// Logique d'interaction pour HomePage.xaml
    /// </summary>
    public partial class HomePage : Window, INavigableWindow
    {
        public HomePage()
        {
            InitializeComponent();
            DataContext = new HomePageViewModel();

            // Définir cette fenêtre comme fenêtre active dans NavigationService
            NavigationService.Instance.SetCurrentWindow(this);
        }

        public void LoadNewUserControl(UserControl view)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(object parameter)
        {
            if (this.DataContext is HomePageViewModel viewModel)
            {
                viewModel.OnNavigatedTo(parameter);
            }
        }
    }
}
