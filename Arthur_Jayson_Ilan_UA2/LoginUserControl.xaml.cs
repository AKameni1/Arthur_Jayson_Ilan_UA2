using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.IconPacks;

namespace Arthur_Jayson_Ilan_UA2
{
    /// <summary>
    /// Logique d'interaction pour LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        public LoginUserControl()
        {
            InitializeComponent();
            Loaded += LoginUserControl_Loaded;
        }

        private void LoginUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("FadeInAnimation");
            storyboard.Begin(this);
        }

        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            var eyeIcon = (PackIconMaterial)toggleButton.Template.FindName("EyeIcon", toggleButton);

            if (toggleButton.IsChecked == true )
            {
                eyeIcon.Kind = PackIconMaterialKind.Eye;

                PasswordTextBox.Text = PasswordBox.Password;
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;

            }
            else
            {
                eyeIcon.Kind = PackIconMaterialKind.EyeOff;

                PasswordBox.Password = PasswordTextBox.Text;
                PasswordBox.Visibility = Visibility.Visible;
                PasswordTextBox.Visibility = Visibility.Collapsed;

            }

        }

        //private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    PasswordTextBox.Text = PasswordBox.Password;
        //}

        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordBox.Password = PasswordTextBox.Text;
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;

            if (string.IsNullOrEmpty(UsernameTextBox.Text))
            {
                UsernameErrorTextBlock.Text = "Veuillez entrer un nom d'utilisateur.";
                UsernameErrorTextBlock.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                UsernameErrorTextBlock.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                PasswordErrorTextBlock.Text = "Veuillez entrer un mot de passe.";
                PasswordErrorTextBlock.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                PasswordErrorTextBlock.Visibility = Visibility.Collapsed;
            }

            if (!hasError)
            {
                var user = App.UserManager.Authenticate(UsernameTextBox.Text, PasswordBox.Password);

                if (user != null)
                {
                    if (user.IsSuperAdmin)
                    {
                        MessageBox.Show("Bienvenue, super administrateur!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Connexion réussie!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Échec de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void NotHaveAccount_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.LoadNewUserControl(new SignupUserControl());
        }
    }
}
