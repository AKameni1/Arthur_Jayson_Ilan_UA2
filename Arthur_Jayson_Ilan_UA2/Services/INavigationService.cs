using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arthur_Jayson_Ilan_UA2.Services
{
    public interface INavigationService
    {
        void NavigateTo(UserControl view);
        void GoBack();

        void OpenWindow<T>() where T : Window, INavigableWindow, new();
        void OpenWindow<T>(object parameter) where T : Window, INavigableWindow, new();
        void CloseWindow(Window window);
    }
}
