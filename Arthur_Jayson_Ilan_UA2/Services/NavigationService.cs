using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Arthur_Jayson_Ilan_UA2.Services
{
    public class NavigationService : INavigationService
    {
        private readonly MainWindow _mainWindow;

        public NavigationService(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void GoBack()
        {
            // Implémenter la logique de retour si nécessaire
            // Par exemple, utiliser une pile pour gérer l'historique des vues
        }

        public void NavigateTo(UserControl view)
        {
            _mainWindow.LoadNewUserControl(view);
        }
    }
}
