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
    /// Logique d'interaction pour SignupView.xaml
    /// </summary>
    public partial class SignupView : UserControl
    {
        public SignupView()
        {
            InitializeComponent();

            // Instancier le ViewModel avec le service de navigation
            var viewModel = new SignupViewModel();

            // Définir le DataContext
            DataContext = viewModel;

            Loaded += SignupView_Loaded;
        }

        private void SignupView_Loaded(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("FadeInAnimation");
            storyboard.Begin(this);
        }
    }
}
