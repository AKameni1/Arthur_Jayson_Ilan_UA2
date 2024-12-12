using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Model;
using Microsoft.EntityFrameworkCore;

namespace Arthur_Jayson_Ilan_UA2
{
    public sealed class LibraryContextManager
    {
        private static readonly Lazy<LibraryContext> _instance = new(() =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlite("Data Source=library.db");
            return new LibraryContext(optionsBuilder.Options);
        });

        private bool _disposed = false;

        /// <summary>
        /// Instance unique de LibraryContext.
        /// </summary>
        public static LibraryContext Instance => _instance.Value;

        /// <summary>
        /// Constructeur privé pour empêcher l'instanciation directe.
        /// </summary>
        private LibraryContextManager() { }

        /// <summary>
        /// Libère les ressources associées au contexte.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                if (_instance.IsValueCreated)
                {
                    _instance.Value.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
