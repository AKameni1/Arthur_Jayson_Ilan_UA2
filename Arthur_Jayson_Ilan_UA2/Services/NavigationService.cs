using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arthur_Jayson_Ilan_UA2.Services
{
    public class NavigationService : INavigationService
    {
        private static NavigationService? _instance;
        private INavigableWindow? _currentWindow;

        public static NavigationService Instance
        {
            get
            {
                _instance ??= new NavigationService();
                return _instance;
            }
        }

        public NavigationService() { }

        // Méthode pour définir la fenêtre active
        public void SetCurrentWindow(INavigableWindow window)
        {
            _currentWindow = window;
        }

        public void GoBack()
        {
            // Implémenter la logique de retour si nécessaire
            // Par exemple, utiliser une pile pour gérer l'historique des vues
        }

        // Naviguer vers un UserControl dans la fenêtre active
        public void NavigateTo(UserControl view)
        {
            _currentWindow?.LoadNewUserControl(view);
        }

        // Ouvrir une nouvelle fenêtre sans paramètre
        public void OpenWindow<T>() where T : Window, INavigableWindow, new()
        {
            T window = new T();
            window.Show();

            if (window is INavigableWindow navigable)
            {
                SetCurrentWindow(navigable);
            }

            // Fermer la fenêtre actuelle si nécessaire
            Application.Current.MainWindow?.Close();

            Application.Current.MainWindow = window;
        }

        // Ouvrir une nouvelle fenêtre avec un paramètre
        public void OpenWindow<T>(object parameter) where T : Window, INavigableWindow, new()
        {
            T window = new T();

            if (window is INavigableWindow navigable)
            {
                navigable.OnNavigatedTo(parameter);
                SetCurrentWindow(navigable);
            }

            window.Show();

            // Fermer la fenêtre actuelle si nécessaire
            Application.Current.MainWindow?.Close();

            Application.Current.MainWindow = window;
        }

        // Fermer une fenêtre spécifique
        public void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
                if (_currentWindow is Window currentWin && currentWin == window)
                {
                    _currentWindow = null;
                }
            }
        }
    }
}
