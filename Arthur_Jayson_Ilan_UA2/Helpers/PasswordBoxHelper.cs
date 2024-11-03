using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arthur_Jayson_Ilan_UA2.Helpers
{
    public class PasswordBoxHelper
    {
        // Propriété attachée pour activer le helper
        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false, Attach));

        // Propriété attachée pour la liaison du mot de passe
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordPropertyChanged));

        // Propriété privée pour éviter les boucles infinies
        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordBoxHelper));

        // Méthodes Get/Set pour la propriété Attach
        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        // Méthodes Get/Set pour la propriété Password
        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        // Méthodes Get/Set pour IsUpdating
        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        // Méthode appelée lorsque AttachProperty change
        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if ((bool)e.OldValue)
                {
                    passwordBox.PasswordChanged -= PasswordChanged;
                }

                if ((bool)e.NewValue)
                {
                    passwordBox.PasswordChanged += PasswordChanged;
                }
            }
        }

        // Méthode appelée lorsque PasswordProperty change
        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is PasswordBox passwordBox))
                return;

            passwordBox.PasswordChanged -= PasswordChanged;

            if (!GetIsUpdating(passwordBox))
            {
                passwordBox.Password = e.NewValue as string ?? string.Empty;
            }

            passwordBox.PasswordChanged += PasswordChanged;
        }

        // Gestionnaire de l'événement PasswordChanged
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!(sender is PasswordBox passwordBox))
                return;

            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
}
