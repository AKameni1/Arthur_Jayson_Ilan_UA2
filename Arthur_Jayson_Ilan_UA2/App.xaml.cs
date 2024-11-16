using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Arthur_Jayson_Ilan_UA2.Data;
using Arthur_Jayson_Ilan_UA2.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Arthur_Jayson_Ilan_UA2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IConfiguration? Configuration { get; private set;  }
        public static LibraryContext? DbContext { get; private set; }
        public static UserService? UserService { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Construire la configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // Configurer les options du DbContext
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Chaîne de connexion 'DefaultConnection' non trouvée dans appsettings.json.");
            }

            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            );

            // Initialiser le DbContext
            DbContext = new LibraryContext(optionsBuilder.Options);

            // Appliquer les migrations automatiquement (optionnel)
            DbContext.Database.Migrate();

            // Initialer les services
            UserService = new UserService(DbContext);
        }
    }

}
