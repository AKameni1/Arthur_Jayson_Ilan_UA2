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
using Arthur_Jayson_Ilan_UA2;

namespace Arthur_Jayson_Ilan_UA2
{
    /// <summary>
    /// Logique d'interaction pour ResetPasswordUserControl.xaml
    /// </summary>
    public partial class ResetPasswordUserControl : UserControl
    {
        private User CurrentUser;
        public ResetPasswordUserControl(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            Loaded += ResetPasswordUserControl_Loaded;
        }

        private void ResetPasswordUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("FadeInAnimation");
            storyboard.Begin(this);
        }

        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            var eyeIcon = (PackIconMaterial)toggleButton.Template.FindName("EyeIcon", toggleButton);

            if (toggleButton == TogglePasswordVisibility) // Pour le mot de passe principal
            {
                if (toggleButton.IsChecked == true)
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
            else if (toggleButton == ToggleConfirmPasswordVisibility) // Pour le mot de passe de confirmation
            {
                if (toggleButton.IsChecked == true)
                {
                    eyeIcon.Kind = PackIconMaterialKind.Eye;
                    ConfirmPasswordTextBox.Text = ConfirmPasswordBox.Password;
                    ConfirmPasswordTextBox.Visibility = Visibility.Visible;
                    ConfirmPasswordBox.Visibility = Visibility.Collapsed;
                }
                else
                {
                    eyeIcon.Kind = PackIconMaterialKind.EyeOff;
                    ConfirmPasswordBox.Password = ConfirmPasswordTextBox.Text;
                    ConfirmPasswordBox.Visibility = Visibility.Visible;
                    ConfirmPasswordTextBox.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordBox.Password = PasswordTextBox.Text;
        }

        private void ConfirmPasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConfirmPasswordBox.Password = ConfirmPasswordTextBox.Text;
        }

        private bool ValidatePassword()
        {
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            PasswordErrorTextBlock.Visibility = Visibility.Collapsed;
            ConfirmPasswordErrorTextBlock.Visibility = Visibility.Collapsed;

            if (password.Length < 6)
            {
                PasswordErrorTextBlock.Text = "Le mot de passe doit contenir au moins 6 caractères.";
                PasswordErrorTextBlock.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                if (password != confirmPassword)
                {
                    ConfirmPasswordErrorTextBlock.Text = "Les mots de passe ne correspondent pas.";
                    ConfirmPasswordErrorTextBlock.Visibility = Visibility.Visible;
                    return false;
                }
            }

            return true;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidatePassword())
            {
                try
                {
                    App.UserManager.UpdatePassword(CurrentUser, PasswordBox.Password);
                    MessageBox.Show("Mot de passe mis à jour avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow?.LoadNewUserControl(new LoginUserControl());
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show($"Erreur de mise à jour : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show($"Erreur de validation : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur inattendue s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
