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
        public readonly static UserService UserService = new();
        public readonly static Service.UserService userService = new();
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Initialiser le SuperAdmin au démarrage
            try
            {
                await userService.CreateSuperAdminIfNotExistsAsync(
                    username: "Admin",
                    email: "admin@example.com",
                    password: "SuperSecurePassword123"
                );
            }
            catch (InvalidOperationException ex)
            {
                // Un SuperAdmin existe déjà, log ou ignorez
                MessageBox.Show(ex.Message, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Log ou afficher une erreur critique
                MessageBox.Show($"Erreur lors de la création du SuperAdmin : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(); // Arrêter l'application en cas d'erreur critique
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            LibraryContextManager.Instance.Dispose();
            base.OnExit(e);
        }
    }

}
