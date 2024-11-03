using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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

        // Propriété attachée pour la liaison du mot de passe sécurisé
        public static readonly DependencyProperty SecurePasswordProperty =
            DependencyProperty.RegisterAttached("SecurePassword", typeof(SecureString), typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(default(SecureString), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSecurePasswordPropertyChanged));

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

        // Méthodes Get/Set pour la propriété SecurePassword
        public static SecureString GetSecurePassword(DependencyObject dp)
        {
            return (SecureString)dp.GetValue(SecurePasswordProperty);
        }

        public static void SetSecurePassword(DependencyObject dp, SecureString value)
        {
            dp.SetValue(SecurePasswordProperty, value);
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

        // Méthode appelée lorsque SecurePasswordProperty change
        private static void OnSecurePasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not PasswordBox passwordBox)
                return;

            passwordBox.PasswordChanged -= PasswordChanged;

            if (!GetIsUpdating(passwordBox))
            {
                if (e.NewValue is SecureString newSecurePassword)
                {
                    passwordBox.Password = ConvertToUnsecureString(newSecurePassword);
                }
                else
                {
                    passwordBox.Password = string.Empty;
                }
            }

            passwordBox.PasswordChanged += PasswordChanged;
        }

        // Gestionnaire de l'événement PasswordChanged
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is not PasswordBox passwordBox)
                return;

            SetIsUpdating(passwordBox, true);
            SetSecurePassword(passwordBox, passwordBox.SecurePassword.Copy());
            SetIsUpdating(passwordBox, false);
        }

        // Méthode pour convertir SecureString en string de manière sécurisée
        private static string ConvertToUnsecureString(SecureString secureString)
        {
            if (secureString == null)
                return string.Empty;

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString) ?? string.Empty;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
