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
using Arthur_Jayson_Ilan_UA2;

namespace Arthur_Jayson_Ilan_UA2
{
    /// <summary>
    /// Logique d'interaction pour ResetUsernameUserControl.xaml
    /// </summary>
    public partial class ResetUsernameUserControl : UserControl
    {
        private User _currentUser;
        public ResetUsernameUserControl(User user)
        {
            InitializeComponent();
            _currentUser = user;
            Loaded += ResetUsernameUserControl_Loaded;
        }

        private void ResetUsernameUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("FadeInAnimation");
            storyboard.Begin(this);
        }

        private bool ValidateUsername()
        {
            string newUsername = UsernameTextBox.Text.Trim();

            UsernameErrorTextBlock.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(newUsername))
            {
                UsernameErrorTextBlock.Text = "Le nom d'utilisateur ne peut pas être vide.";
                UsernameErrorTextBlock.Visibility = Visibility.Visible;
                return false;
            }

            if (App.UserManager.UsernameExists(newUsername))
            {
                UsernameErrorTextBlock.Text = "Ce nom d'utilisateur est déjà pris.";
                UsernameErrorTextBlock.Visibility = Visibility.Visible;
                return false;
            }

            return true;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateUsername())
            {
                try
                {
                    _currentUser.ChangeUsername(UsernameTextBox.Text.Trim(), App.UserManager);
                    MessageBox.Show("Nom d'utilisateur mis à jour avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow?.LoadNewUserControl(new LoginUserControl());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur : {ex.Message}");
                }
            }
        }
    }
}
