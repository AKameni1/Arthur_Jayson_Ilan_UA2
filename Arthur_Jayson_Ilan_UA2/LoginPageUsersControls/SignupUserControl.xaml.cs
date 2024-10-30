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
    /// Logique d'interaction pour SignupUserControl.xaml
    /// </summary>
    public partial class SignupUserControl : UserControl
    {
        public SignupUserControl()
        {
            InitializeComponent();
            Loaded += SignupUserControl_Loaded;
        }

        private void SignupUserControl_Loaded(object sender, RoutedEventArgs e)
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

        private bool ValidateFields()
        {
            bool isValid = true;
            string emailPattern = @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,6}$";
            UsernameErrorTextBlock.Visibility = Visibility.Collapsed;
            EmailErrorTextBlock.Visibility = Visibility.Collapsed;
            PasswordErrorTextBlock.Visibility = Visibility.Collapsed;
            ConfirmPasswordErrorTextBlock.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                UsernameErrorTextBlock.Text = "Veuillez entrer un nom d'utilisateur.";
                UsernameErrorTextBlock.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(EmailTextBox.Text) || !Regex.IsMatch(EmailTextBox.Text, emailPattern))
            {
                EmailErrorTextBlock.Text = "Veuillez entrer une adresse e-mail valide.";
                EmailErrorTextBlock.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                PasswordErrorTextBlock.Text = "Veuillez entrer un mot de passe.";
                PasswordErrorTextBlock.Visibility = Visibility.Visible;
                isValid = false;
            }
            else if (PasswordBox.Password.Length < 12)
            {
                PasswordErrorTextBlock.Text = "Le mot de passe doit comporter au moins 12 caractères.";
                PasswordErrorTextBlock.Visibility = Visibility.Visible;
                isValid = false;
            }
            else if (!(PasswordBox.Password.Any(char.IsUpper) && PasswordBox.Password.Any(char.IsLower) && PasswordBox.Password.Any(char.IsDigit)))
            {
                PasswordErrorTextBlock.Text = "Le mot de passe doit contenir au moins une majuscule, une minuscule, et un chiffre.";
                PasswordErrorTextBlock.Visibility = Visibility.Visible;
                isValid =  false;
            }
            else
            {
                PasswordErrorTextBlock.Visibility = Visibility.Collapsed;

                if (PasswordBox.Password != ConfirmPasswordBox.Password)
                {
                    ConfirmPasswordErrorTextBlock.Text = "Les mots de passe ne correspondent pas.";
                    ConfirmPasswordErrorTextBlock.Visibility = Visibility.Visible;
                    isValid = false;
                }

            }

            return isValid;
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                try
                {
                    SignupErrorTextBlock.Visibility = Visibility.Collapsed;
                    App.UserManager.RegisterUser(UsernameTextBox.Text, PasswordBox.Password, EmailTextBox.Text);

                    await Task.Delay(200);
                    //MessageBox.Show("Inscription réussie !");
                    SignupSuccessTextBlock.Text = "Inscription réussie !";
                    SignupSuccessTextBlock.Visibility = Visibility.Visible;

                    await Task.Delay(1000);

                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow?.LoadNewUserControl(new LoginUserControl());
                    SignupSuccessTextBlock.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    SignupErrorTextBlock.Text = ex.Message;
                    SignupErrorTextBlock.Visibility= Visibility.Visible;
                }
            }
        }

        private void AlreadyHaveAccount_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.LoadNewUserControl(new LoginUserControl());
        }
    }
}
