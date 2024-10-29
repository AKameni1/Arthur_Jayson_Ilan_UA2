using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logique d'interaction pour EmailVerificationUserControl.xaml
    /// </summary>
    public partial class EmailVerificationUserControl : UserControl
    {
        private readonly string _resetType;
        public EmailVerificationUserControl(string resetType)
        {
            InitializeComponent();
            _resetType = resetType;
            Loaded += EmailVerificationUserControl_Loaded;
        }

        private void EmailVerificationUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("FadeInAnimation");
            storyboard.Begin(this);
            ResetAuthFields();
        }


        private void ResetAuthFields()
        {
            EmailVerification.Visibility = Visibility.Visible;
            CheckOut.Visibility = Visibility.Visible;
            SuperAdminAuthFields.Visibility = Visibility.Collapsed;
            SuperAdminCheckout.Visibility = Visibility.Collapsed;
            Container.MaxHeight = 230;
        }
        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            var eyeIcon = (PackIconMaterial)toggleButton.Template.FindName("EyeIcon", toggleButton);

            if (toggleButton.IsChecked == true)
            {
                eyeIcon.Kind = PackIconMaterialKind.Eye;

                SuperAdminPasswordTextBox.Text = SuperAdminPasswordBox.Password;
                SuperAdminPasswordTextBox.Visibility = Visibility.Visible;
                SuperAdminPasswordBox.Visibility = Visibility.Collapsed;

            }
            else
            {
                eyeIcon.Kind = PackIconMaterialKind.EyeOff;

                SuperAdminPasswordBox.Password = SuperAdminPasswordTextBox.Text;
                SuperAdminPasswordBox.Visibility = Visibility.Visible;
                SuperAdminPasswordTextBox.Visibility = Visibility.Collapsed;

            }

        }

        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SuperAdminPasswordBox.Password = SuperAdminPasswordTextBox.Text;
        }

        private void ShowSuperAdminAuthFields()
        {
            Container.MaxHeight = 320;
            EmailVerification.Visibility = Visibility.Collapsed;
            CheckOut.Visibility = Visibility.Collapsed;
            SuperAdminAuthFields.Visibility = Visibility.Visible;
            SuperAdminCheckout.Visibility = Visibility.Visible;

            MessageBox.Show("Veuillez entrer les informations de connexion pour le super admin.", "Authentification requise", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void VerifyEmailButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = ValidateEmail();

            if (isValid)
            {
                var user = App.UserManager.FindUserByEmail(EmailTextBox.Text.Trim());

                if (user != null)
                {

                    if (user.IsSuperAdmin)
                    {
                        ShowSuperAdminAuthFields();
                    }
                    else
                    {
                        ProceedToReset(user);
                    }
                }
                else
                {
                    ShowEmailError("Cette adresse e-mail n'est pas enregistrée.");
                }
            }
        }

        private void AuthenticateSuperAdminButton_Click(object sender, RoutedEventArgs e)
        {
            var superAdmin = App.UserManager.FindUserByEmail(EmailTextBox.Text.Trim());

            if (superAdmin != null && superAdmin.VerifyPassword(SuperAdminPasswordBox.Password) && superAdmin.Username == SuperAdminUsernameTextBox.Text.Trim())
            {
                ProceedToReset(superAdmin);
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Échec de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ProceedToReset(User user)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (_resetType == "password")
            {
                MessageBox.Show("Adresse e-mail vérifiée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                mainWindow?.LoadNewUserControl(new ResetPasswordUserControl(user));
            }
            else if (_resetType == "username")
            {
                MessageBox.Show("Adresse e-mail vérifiée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                mainWindow?.LoadNewUserControl(new ResetUsernameUserControl(user));
            }
        }

        private bool ValidateEmail()
        {
            bool isValid = true;
            string emailPattern = @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,6}$";
            string emailInput = EmailTextBox.Text.Trim();

            if (string.IsNullOrEmpty(emailInput) || !Regex.IsMatch(emailInput, emailPattern))
            {
                ShowEmailError("Veuillez entrer une adresse e-mail valide");
                isValid = false;
            }
            else
            {
                EmailErrorTextBlock.Visibility= Visibility.Collapsed;
            }

            return isValid;
        }

        private void ShowEmailError(string message)
        {
            EmailErrorTextBlock.Text = message;
            EmailErrorTextBlock.Visibility = Visibility.Visible;
        }
    }
}
