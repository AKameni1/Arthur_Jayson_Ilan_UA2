using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Arthur_Jayson_Ilan_UA2.ViewsModels.HomePageViewModels;


namespace Arthur_Jayson_Ilan_UA2.Views.HomePageViews.SupportViews
{
    /// <summary>
    /// Logique d'interaction pour TechnicalAssistanceView.xaml
    /// </summary>
    public partial class TechnicalAssistanceView : UserControl
    {
        public TechnicalAssistanceView()
        {
            InitializeComponent();
            DataContext = new TechnicalAssitanceViewModel();
        }

        private void EmailTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var email = "arthur.kamenitchualeu@gmail.com";
            Process.Start(new ProcessStartInfo($"mailto:{email}") { UseShellExecute = true });
        }

        private void PhoneTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var phoneNumber = "+16132183956";
            Process.Start(new ProcessStartInfo($"tel:{phoneNumber}") { UseShellExecute = true });
        }
    }
}
