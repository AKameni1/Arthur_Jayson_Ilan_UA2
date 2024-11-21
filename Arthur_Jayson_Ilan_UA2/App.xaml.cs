using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Arthur_Jayson_Ilan_UA2.Data;
using Arthur_Jayson_Ilan_UA2.Models;
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
        public static UserService UserService { get; private set; } = null!;
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
                ServerVersion.AutoDetect(connectionString),
                mySqlOptions =>
                {
                    mySqlOptions.EnableStringComparisonTranslations();
                }
            );

            // Initialiser le DbContext
            DbContext = new LibraryContext(optionsBuilder.Options);

            // Tester la connexion
            //if (!await DbContext.Database.CanConnectAsync())
            //{
            //    throw new InvalidOperationException("Impossible de se connecter à la base de données avec la chaîne de connexion fournie.");
            //}

            // Appliquer les migrations automatiquement (optionnel)
            DbContext.Database.Migrate();

            //// Vérifier que les rôles nécessaires existent, sinon les ajouter
            //var roles = new List<Role>
            //    {
            //        new Role { RoleID = 1, Name = "superAdmin", Description = "Super administrateur avec tous les droits d'administration, y compris la gestion des utilisateurs et des rôles" },
            //        new Role { RoleID = 2, Name = "admin", Description = "Administrateur avec la possibilité de gérer les utilisateurs et les permissions" },
            //        new Role { RoleID = 3, Name = "librarian", Description = "Bibliothécaire ayant des droits sur les livres et les emprunts" },
            //        new Role { RoleID = 4, Name = "client", Description = "Utilisateur standard avec des droits d'emprunt de livres et de consultation" }
            //    };

            //foreach (var role in roles)
            //{
            //    if (!await DbContext.Roles.AnyAsync(r => r.RoleID == role.RoleID))
            //    {
            //        DbContext.Roles.Add(role);
            //    }
            //}

            //await DbContext.SaveChangesAsync();

            // Initialer les services
            UserService = new UserService(DbContext);
        }
    }

}
