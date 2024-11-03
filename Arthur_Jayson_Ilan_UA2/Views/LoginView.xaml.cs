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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Arthur_Jayson_Ilan_UA2.ViewsModels;

namespace Arthur_Jayson_Ilan_UA2.Views
{
    /// <summary>
    /// Logique d'interaction pour LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();

            // Récupérer une référence au MainWindow
            var mainWindow = Application.Current.MainWindow as MainWindow ?? throw new InvalidOperationException("MainWindow is not available.");

            // Instancier le service de navigation avec la référence au MainWindow
            var navigationService = new Services.NavigationService(mainWindow);

            // Instancier le ViewModel avec le service de navigation
            var viewModel = new LoginViewModel(navigationService);

            // Définir le DataContext
            DataContext = viewModel;

            Loaded += LoginView_Loaded;
        }

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("FadeInAnimation");
            storyboard.Begin(this);
        }
    }
}
