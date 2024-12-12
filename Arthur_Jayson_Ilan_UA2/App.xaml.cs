using System.Configuration;
using System.Data;
using System.Windows;
using Arthur_Jayson_Ilan_UA2.Model;
using Arthur_Jayson_Ilan_UA2.Models;

namespace Arthur_Jayson_Ilan_UA2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UserService UserService { get; } = new UserService();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            LibraryContextManager.Instance.Dispose();
            base.OnExit(e);
        }
    }

}
