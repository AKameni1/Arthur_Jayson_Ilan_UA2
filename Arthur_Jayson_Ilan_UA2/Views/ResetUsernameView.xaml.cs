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
using Arthur_Jayson_Ilan_UA2.Models;
using Arthur_Jayson_Ilan_UA2.ViewsModels;

namespace Arthur_Jayson_Ilan_UA2.Views
{
    /// <summary>
    /// Logique d'interaction pour ResetUsernameView.xaml
    /// </summary>
    public partial class ResetUsernameView : UserControl
    {
        public ResetUsernameView(User user)
        {
            InitializeComponent();

            // Instancier le ViewModel avec le service de navigation, le UserService et l'utilisateur actuel
            var viewModel = new ResetUsernameViewModel(user);

            // Définir le DataContext
            this.DataContext = viewModel;

            // Gérer l'événement Loaded pour démarrer l'animation
            Loaded += ResetUsernameView_Loaded;
        }

        private void ResetUsernameView_Loaded(object sender, RoutedEventArgs e)
        {
            if (FindResource("FadeInAnimation") is Storyboard storyboard)
            {
                storyboard.Begin(this);
            }
        }
    }
}
