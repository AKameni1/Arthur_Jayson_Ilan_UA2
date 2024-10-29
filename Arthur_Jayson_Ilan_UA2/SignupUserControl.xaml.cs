﻿using System;
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

            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                UsernameErrorTextBlock.Text = "Veuillez entrer un nom d'utilisateur.";
                UsernameErrorTextBlock.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                UsernameErrorTextBlock.Visibility = Visibility.Collapsed;
                //UsernameErrorTextBlock.Text = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(EmailTextBox.Text) || !Regex.IsMatch(EmailTextBox.Text, emailPattern))
            {
                EmailErrorTextBlock.Text = "Veuillez entrer une adresse e-mail valide.";
                EmailErrorTextBlock.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                EmailErrorTextBlock.Visibility = Visibility.Collapsed;
                //EmailErrorTextBlock.Text = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                PasswordErrorTextBlock.Text = "Veuillez entrer un mot de passe.";
                PasswordErrorTextBlock.Visibility = Visibility.Visible;
                isValid = false;
            }
            else if (PasswordBox.Password.Length < 6)
            {
                PasswordErrorTextBlock.Text = "Le mot de passe doit comporter au moins 6 caractères.";
                PasswordErrorTextBlock.Visibility = Visibility.Visible;
                isValid = false;
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
                else
                {
                    ConfirmPasswordErrorTextBlock.Visibility = Visibility.Collapsed;
                    //ConfirmPasswordErrorTextBlock.Text = string.Empty;
                }

                //PasswordErrorTextBlock.Text = string.Empty;

            }

            return isValid;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                try
                {
                    App.UserManager.RegisterUser(UsernameTextBox.Text, PasswordBox.Password, EmailTextBox.Text);
                    MessageBox.Show("Inscription réussie !");

                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow?.LoadNewUserControl(new LoginUserControl());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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