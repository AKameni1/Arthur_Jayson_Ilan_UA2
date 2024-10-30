using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arthur_Jayson_Ilan_UA2
{
    class CommonMethods
    {
        public static void ReturnToLoginUserControl()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.LoadNewUserControl(new LoginUserControl());
        }
    }
}
