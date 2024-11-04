using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Arthur_Jayson_Ilan_UA2.Services
{
    public interface INavigableWindow
    {
        void LoadNewUserControl(UserControl view);
        void OnNavigatedTo(object parameter);
    }
}
