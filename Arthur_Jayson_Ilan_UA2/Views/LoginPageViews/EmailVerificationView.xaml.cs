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
using Arthur_Jayson_Ilan_UA2.Services;
using Arthur_Jayson_Ilan_UA2.ViewsModels;
using Arthur_Jayson_Ilan_UA2.ViewsModels.LoginPageViewModels;

namespace Arthur_Jayson_Ilan_UA2.Views
{
    /// <summary>
    /// Logique d'interaction pour EmailVerificationView.xaml
    /// </summary>
    public partial class EmailVerificationView : UserControl
    {
        public EmailVerificationView(string resetType)
        {
            InitializeComponent();

            // Instancier le ViewModel avec le service de navigation
            var viewModel = new EmailVerificationViewModel();
            viewModel.SetResetType(resetType);

            // Définir le DataContext
            DataContext = viewModel;

            Loaded += EmailVerificationView_Loaded;
        }

        private void EmailVerificationView_Loaded(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("FadeInAnimation");
            storyboard.Begin(this);
        }
    }
}
